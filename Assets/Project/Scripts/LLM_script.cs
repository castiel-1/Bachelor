using System.Collections.Generic;
using System.Threading.Tasks;
using LLMUnity;
using UnityEditor;
using UnityEngine;

public class LLM_script : MonoBehaviour
{
    [SerializeField]
    LLMCharacter llm;

    public enum GenerateOptions
    {
        GenerateWords,
        GenerateSentences
    }

    [SerializeField]
    GenerateOptions generateOptions;

    [SerializeField]
    string startInput;

    async void Start()
    {

        Tree_script treeScript = GetComponent<Tree_script>();

        Debug.Log("Generating tree...");
        List<object> tree = treeScript.GenerateRandomTree(0);

        Debug.Log("Formatting tree...");
        Debug.Log("Tree without words:\n" + treeScript.FormatTree(tree, 0));

        await PopulateTreeAsync(tree, startInput);
        Debug.Log("Formatting tree...");
        Debug.Log("Final tree:\n" + treeScript.FormatTree(tree, 0));
    }

    async Task PopulateTreeAsync(List<object> tree, string startInput)
    {
        Debug.Log("Populating tree...");

        tree.RemoveAt(0);

        await PopulateChildTreeAsync(tree, startInput);

        tree.Insert(0, startInput);
    }
    async Task PopulateChildTreeAsync(List<object> tree, string sentence)
    {

        for (int i = 0; i < tree.Count; i++) {

            if (tree[i] is List<object> childTree)
            {
                await PopulateChildTreeAsync(childTree, sentence);
            }
            else
            {
                string nextContent = "";

                if(generateOptions == GenerateOptions.GenerateWords)
                {
                    nextContent = await GenerateWordAsync(sentence);
                    nextContent = nextContent.ToLowerInvariant();

                }
                else if (generateOptions  == GenerateOptions.GenerateSentences)
                {
                    nextContent = await GenerateSentenceAsync(sentence);
                }
                else
                {
                    Debug.LogError("No option picked for generating content.");
                }

                tree[i] = nextContent;

                sentence = $"{sentence} {nextContent}".Trim();

            }

        }
    }

    async Task<string> GenerateWordAsync(string sentence)
    {
        string prompt = $"Forget the previous prompt. Generate a word. The generated word should " +
            $"continue the sentence: '{sentence}'. Your answer should only be the generated word.";
        string reply = await llm.Chat(prompt);
        Debug.Log("Prompt: " + prompt);
        Debug.Log("Generated word: " + reply);
        return reply;
    }

    async Task<string> GenerateSentenceAsync(string sentence)
    {
        string prompt = $"Forget the previous prompt and your previous answers. Generate a sentence at most 5 words long. The generated sentence should " +
            $"continue the story based on this start and nothing else: '{sentence}'. Your answer should only be your newly generated sentence.";
        string reply = await llm.Chat(prompt);
        Debug.Log("Prompt: " + prompt);
        Debug.Log("Generated word: " + reply);
        return reply;
    }

}
