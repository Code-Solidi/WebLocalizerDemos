/*
 * Copyright Code Solidi Ltd. (c) 2021, 2022. All rights reserved.
 */

using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebLocalizerBlazorSrvDemo.Components
{
    /// <summary>
    /// The observer.
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// Receive update from subject
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="args"></param>
        Task UpdateAsync(INotifierService subject, params object[] args);
    }

    /// <summary>
    /// The subject.
    /// </summary>
    public interface INotifierService
    {
        /// <summary>
        /// Attach an observer to the subject.
        /// </summary>
        /// <param name="observer">The observer.</param>
        void Attach(IObserver observer);

        /// <summary>
        /// Detach an observer from the subject.
        /// </summary>
        /// <param name="observer">The observer.</param>
        void Detach(IObserver observer);

        /// <summary>
        /// Notify all observers about an event.
        /// </summary>
        /// <param name="args"></param>
        Task NotifyAsync(params object[] args);
    }

    /// <summary>
    /// The Subject owns some important state and notifies observers when the state changes.
    /// </summary>
    public class NotifierService : INotifierService
    {
        private readonly List<IObserver> observers = new List<IObserver>();

        /// <summary>
        /// The subscription management methods.
        /// </summary>
        /// <param name="observer">The observer.</param>
        public void Attach(IObserver observer)
        {
            this.observers.Add(observer);
        }

        /// <summary>
        /// Detaches the observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        public void Detach(IObserver observer)
        {
            this.observers.Remove(observer);
        }

        /// <summary>
        /// Trigger an update in each subscriber.
        /// </summary>
        /// <param name="args"></param>
        public async Task NotifyAsync(params object[] args)
        {
            foreach (var observer in observers)
            {
                await observer.UpdateAsync(this, args);
            }
        }
    }
}