﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Developing.Graphs
{
    public class DirectedGraph : IGraph
    {
        /// <inheritdoc/>
        public int VertexCount { get; private set; }

        /// <inheritdoc/>
        public IGraphRepresentation Representation { get; protected set; }

        /// <inheritdoc/>
        public virtual bool Directed => true;

        protected readonly IAdjacencyStructure AdjacencyStructure;

        public DirectedGraph(int vertices, IGraphRepresentation representation)
        {
            Representation = representation;
            VertexCount = vertices;
            AdjacencyStructure = representation.GetAdjacencyStructure<bool>(vertices);
        }

        public DirectedGraph(int vertices) : this(vertices, new DictionaryGraphRepresentation())
        {
        }

        public DirectedGraph(int vertices, IGraphRepresentation graph, IAdjacencyStructure adjacency)
        {
            VertexCount = vertices;
            Representation = graph;
            AdjacencyStructure = adjacency;
        }
        
        public DirectedGraph(IGraph graph)
        {
            for (int i = 0; i < graph.VertexCount; i++)
                foreach (var outNeighbor in graph.OutNeighbors(i))
                    AddEdge(i, outNeighbor);
        }

        public virtual bool AddEdge(int u, int v)
        {
            if (u < 0 || v < 0 || u >= VertexCount || v >= VertexCount)
                throw new IndexOutOfRangeException();

            if (u == v)
                throw new ArgumentException("Can't add an edge connecting the same vertex");

            if (HasEdge(u, v))
                return false;

            AdjacencyStructure.AddEdge(u, v);

            return true;
        }

        /// <inheritdoc/>
        public virtual bool HasEdge(int u, int v)
        {
            if (u < 0 || v < 0 || u >= VertexCount || v >= VertexCount)
                throw new IndexOutOfRangeException();

            return u != v && AdjacencyStructure.HasEdge(u, v);
        }

        /// <inheritdoc/>
        public IEnumerable<int> OutNeighbors(int v)
        {
            if (v < 0 || v >= VertexCount)
                throw new IndexOutOfRangeException();

            foreach (var neighbor in AdjacencyStructure.OutNeighbors(v))
                yield return neighbor;
        }

        public virtual bool RemoveEdge(int u, int v)
        {
            if (u < 0 || u >= VertexCount)
                throw new IndexOutOfRangeException();

            if (AdjacencyStructure.HasEdge(u, v))
                return false;

            AdjacencyStructure.RemoveEdge(u, v);

            return true;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            for (int u = 0; u < this.VertexCount; u++)
            {
                stringBuilder.Append(u);
                stringBuilder.Append(':');

                bool flag = true;
                foreach (var outNeighbor in this.OutNeighbors(u))
                {
                    if (!flag) stringBuilder.Append(' ');
                        
                    flag = false;
                    stringBuilder.Append(outNeighbor);
                }

                if (u < this.VertexCount - 1)
                    stringBuilder.Append('\n');
            }

            return stringBuilder.ToString();
        }
    }
}
