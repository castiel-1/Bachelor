using System.Collections.Generic;
using System.Threading.Tasks;
using LLMUnity;
using UnityEditor;
using UnityEngine;

public class LLM_script : MonoBehaviour
{
    [SerializeField]
    LLMCharacter llm;

    async void Start()
    {
        Tree_script treeScript = GetComponent<Tree_script>();

        Debug.Log("Generating tree...");
        List<object> tree = treeScript.GenerateRandomTree(0);

        Debug.Log("Formatting tree...");
        Debug.Log("Tree without words:\n" + treeScript.FormatTree(tree, 0));

        await PopulateTreeAsync(tree, "You");
        Debug.Log("Formatting tree...");
        Debug.Log("Final tree:\n" + treeScript.FormatTree(tree, 0));
    }

    async Task PopulateTreeAsync(List<object> tree, string startWord)
    {
        Debug.Log("Populating tree...");

        tree.RemoveAt(0);

        await PopulateChildTreeAsync(tree, startWord);

        tree.Insert(0, startWord);
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

                string nextWord = await GenerateWordAsync(sentence);
                string nextWordLowerCase = nextWord.ToLowerInvariant();

                tree[i] = nextWordLowerCase;

                sentence = $"{sentence} {nextWordLowerCase}".Trim();

            }

        }
    }

    async Task<string> GenerateWordAsync(string sentence)
    {
        string prompt = $"Forget the previous prompt. Generate a word. The generated word should continue the sentence: '{sentence}'. Your answer should only be the generated word.";
        string reply = await llm.Chat(prompt);
        Debug.Log("Prompt: " + prompt);
        Debug.Log("Generated word: " + reply);
        return reply;
    }

}
