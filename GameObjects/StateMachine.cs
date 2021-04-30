using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poloknightse
{
    public abstract class State
    {
        public Dictionary<string, Tuple<Func<object, bool>, object>> connections;

        public string name;

        public State(string name)
        {
            this.name = name;
            connections = new Dictionary<string, Tuple<Func<object, bool>, object>>();
        }

        public virtual void Start() { }

        public virtual void FixedUpdate(GameTime gameTime) { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void End() { }
    }


    class StateMachine
    {
        private Dictionary<string, State> states = new Dictionary<string, State>();
        public State CurrentState { private set; get; }

        /// <summary>
        /// Add a <see cref="State"/> state to the dictiorary with <see cref="State.name"/> as key
        /// </summary>
        /// <param name="newState">The state to add</param>
        public void AddState(State newState)
        {
            if (states.ContainsKey(newState.name)) states.Remove(newState.name);
            states.Add(newState.name, newState);
        }

        public void RemoveState(string stateName)
        {
            foreach (string stateKey in states.Keys)
            {
                states[stateKey].connections.Remove(stateName);
            }
            states.Remove(stateName);
        }

        /// <summary>
        /// Sets the current state
        /// </summary>
        /// <param name="newState">The state that will be set</param>
        public void SetState(string newState)
        {
            if (!states.ContainsKey(newState))
                throw new Exception("state not found in list");
            if (CurrentState != null) CurrentState.End();
            CurrentState = states[newState];
            CurrentState.Start();

        }

        /// <summary>
        /// Gets a state from the dictionary
        /// </summary>
        /// <param name="name">The name of the state you wanna get</param>
        /// <returns>The requested <see cref="State"/></returns>
        public State GetState(string name)
        {
            return states[name];
        }

        /// <summary>
        /// Add a connection between states in the dictionary that will switch to the selected state
        /// </summary>
        /// <param name="fromState">Connection from this state</param>
        /// <param name="toState">Connection to this state</param>
        /// <param name="func">The function that checks when to switch</param>
        /// <param name="referanceVariables">Variables to keep track of for the function</param>
        public void AddConnection(string fromState, string toState, Func<object, bool> func, State referanceVariables)
        {
            if (states.ContainsKey(fromState) && states.ContainsKey(toState))
            {
                states[fromState].connections.Add(toState, new Tuple<Func<object, bool>, object>(func, referanceVariables));
            }
        }

        /// <summary>
        /// Add a connection between states in the dictionary that will switch to the selected state
        /// </summary>
        /// <param name="fromState">Connection from this state</param>
        /// <param name="toState">Connection to this state</param>
        /// <param name="func">The function that checks when to switch</param>
        public void AddConnection(string fromState, string toState, Func<bool> func)
        {
            Func<object, bool> func1 = new Func<object, bool>((object var) => func.Invoke());
            if (states.ContainsKey(fromState) && states.ContainsKey(toState))
            {
                states[fromState].connections.Add(toState, new Tuple<Func<object, bool>, object>(func1, null));
            }
        }

        /// <summary>
        /// Add a connection from all states to <paramref name="toState"/> in the dictionary
        /// </summary>
        /// <param name="toState">Connection to this state</param>
        /// <param name="func">The function that checks when to switch</param>
        public void AddConnectionToAll(string toState, Func<bool> func)
        {
            Func<object, bool> func1 = new Func<object, bool>((object var) => func.Invoke());
            if (states.ContainsKey(toState))
            {
                foreach (string state in states.Keys)
                {
                    if (state == toState) continue;
                    if (states[state].connections.ContainsKey(toState)) continue;
                    states[state].connections.Add(toState, new Tuple<Func<object, bool>, object>(func1, null));
                }
            }
        }

        /// <summary>
        /// Add a connection from all states to <paramref name="toState"/> in the dictionary
        /// </summary>
        /// <param name="toState">Connection to this state</param>
        /// <param name="func">The function that checks when to switch</param>
        public void AddConnectionToAll(string toState, Func<object, bool> func, State referanceVariables)
        {
            if (states.ContainsKey(toState))
            {
                foreach (string state in states.Keys)
                {
                    if (state == toState) continue;
                    states[state].connections.Add(toState, new Tuple<Func<object, bool>, object>(func, referanceVariables));
                }
            }
        }

        /// <summary>
        /// Update the current state
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (CurrentState != null)
            {
                CurrentState.Update(gameTime);
                CheckConnections();
            }
        }

        /// <summary>
        /// Update the current state on a fixed time
        /// </summary>
        /// <param name="gameTime"></param>
        public void FixedUpdate(GameTime gameTime)
        {
            if (CurrentState != null)
            {
                CurrentState.FixedUpdate(gameTime);
                CheckConnections();
            }
        }

        /// <summary>
        /// Checks if connection from current state wants to switch
        /// </summary>
        private void CheckConnections()
        {
            foreach (string otherState in CurrentState.connections.Keys)
            {
                Func<object, bool> func = CurrentState.connections[otherState].Item1;

                object args = CurrentState.connections[otherState].Item2;

                if (func.Invoke(args))
                {
                    SetState(otherState);
                }
            }
        }
    }
}
