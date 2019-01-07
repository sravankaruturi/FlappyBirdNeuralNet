
using System.Collections.Generic;
using System;
using UnityEngine;

public class NeuralNetworkTest : IComparable<NeuralNetworkTest>
{

    // Layers contain the number of neurons in the Layer.
    private int[] layers;

    private float[][] neurons;
    private float[][][] weights;
    private float fitness; //fitness of the network

    private System.Random random;

    /// <summary>
    /// Deep copy constructor 
    /// </summary>
    /// <param name="copyNetwork">Network to deep copy</param>
    public NeuralNetworkTest(NeuralNetworkTest copyNetwork)
    {
        random = new System.Random();

        this.layers = new int[copyNetwork.layers.Length];
        for (int i = 0; i < copyNetwork.layers.Length; i++)
        {
            this.layers[i] = copyNetwork.layers[i];
        }

        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.weights);
       
    }

    private void CopyWeights(float[][][] copyWeights)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    public NeuralNetworkTest(int[] _layers)
    {
        random = new System.Random();

        this.layers = new int[_layers.Length];
        for ( int i = 0; i < _layers.Length; i++)
        {
            this.layers[i] = _layers[i];
        }

        random = new System.Random(System.DateTime.Today.Millisecond);

        InitNeurons();
        InitWeights();
    }

    private void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();
        for ( int i = 0; i < layers.Length; i++)
        {
            neuronsList.Add(new float[layers[i]]);
        }
        neurons = neuronsList.ToArray();
    }

    private void InitWeights()
    {
        List<float[][]> weightList = new List<float[][]>();

        for ( int i = 1; i< layers.Length; i++)
        {
            List<float[]> layerWeightList = new List<float[]>();

            int neuronsInThePreviousLayer = layers[i - 1];

            // Go through each neuron of a particular layer.
            for ( int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInThePreviousLayer];

                for ( int k = 0; k < neuronsInThePreviousLayer; k++)
                {
                    // Randomize stuff.
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }

                layerWeightList.Add(neuronWeights);

            }
            weightList.Add(layerWeightList.ToArray());
        }

        weights = weightList.ToArray();

    }

    // Pass in and populate the first Layer.
    public float[] FeedForward(float[] _inputs)
    {
        for ( int i = 0; i < _inputs.Length; i++)
        {
            neurons[0][i] = _inputs[i];
        }

        for ( int i = 1; i < layers.Length; i++)
        {
            for ( int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0.25f;
                for ( int k = 0; k < neurons[i-1].Length; k++)
                {
                    if (float.IsNaN(neurons[i - 1][k]))
                    {
                        neurons[i - 1][k] = 1;
                    }
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = (float)Math.Tanh(value);
            }
        }

        //Debug.Assert(neurons[neurons.Length - 1][0] >= 0.0f);
        //Debug.Log(neurons[neurons.Length - 1][0]);
        return neurons[neurons.Length - 1];
    }

    /// <summary>
    /// Mutate neural network weights
    /// </summary>
    public void Mutate()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    //mutate weight value 
                    float randomNumber = UnityEngine.Random.Range(0f, 100f);

                    if (randomNumber <= 2f)
                    { //if 1
                      //flip sign of weight
                        weight *= -1f;
                    }
                    else if (randomNumber <= 4f)
                    { //if 2
                      //pick random weight between -1 and 1
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (randomNumber <= 6f)
                    { //if 3
                      //randomly increase by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        weight *= factor;
                    }
                    else if (randomNumber <= 8f)
                    { //if 4
                      //randomly decrease by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f);
                        weight *= factor;
                    }

                    weights[i][j][k] = weight;
                }
            }
        }
    }

    public void SetFitness(float _fit)
    {
        fitness = _fit;
    }

    /// <summary>
    /// Compare two neural networks and sort based on fitness
    /// </summary>
    /// <param name="other">Network to be compared to</param>
    /// <returns></returns>
    public int CompareTo(NeuralNetworkTest other)
    {
        if (other == null) return 1;

        if (fitness > other.fitness)
            return 1;
        else if (fitness < other.fitness)
            return -1;
        else
            return 0;
    }
}