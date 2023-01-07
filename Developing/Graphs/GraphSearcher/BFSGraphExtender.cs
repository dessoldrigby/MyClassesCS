﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Developing.Lists;
using Developing.Interfaces;

namespace Developing.Graphs
{
    public static class BFSGraphExtender
    {
        public static IGraphSearcher BFS(this Graph graph) 
            => new GeneralGraphSearcher(graph, new DlQueue<int>(), true);

        public static IGraphSearcher<T> BFS<T>(this Graph<T> graph) where T : new() 
            => new GeneralGraphSearcher<T>(graph, new DlQueue<int>(), true);

        public static IGraphSearcher BFS(this DirectedGraph graph) 
            => new GeneralGraphSearcher(graph, new DlQueue<int>(), false);

        public static IGraphSearcher<T> BFS<T>(this DirectedGraph<T> graph) where T : new() 
            => new GeneralGraphSearcher<T>(graph, new DlQueue<int>(), false);
    }
}
