﻿using System;
using System.Windows.Input;

namespace ImageToLockscreen.Ui.Models
{
    // https://www.codeproject.com/Tips/813345/Basic-MVVM-and-ICommand-Usage-Example
    public class RelayCommand<T> : ICommand
    {
        #region Fields

        /// <summary>
        /// Encapsulated the execute action
        /// </summary>
        private Action<T> execute;

        /// <summary>
        /// Encapsulated the representation for the validation of the execute method
        /// </summary>
        private Predicate<object> canExecute;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RelayCommand class
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<T> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<object> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute ?? throw new ArgumentNullException("canExecute");
        }

        #endregion Constructors

        #region ICommand Members

        /// <summary>
        /// An event to raise when the CanExecute value is changed
        /// </summary>
        /// <remarks>
        /// Any subscription to this event will automatically subscribe to both 
        /// the local OnCanExecuteChanged method AND
        /// the CommandManager RequerySuggested event
        /// </remarks>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        /// <summary>
        /// An event to allow the CanExecuteChanged event to be raised manually
        /// </summary>
        private event EventHandler CanExecuteChangedInternal;

        /// <summary>
        /// Defines if command can be executed
        /// </summary>
        /// <param name="parameter">the parameter that represents the validation method</param>
        /// <returns>true if the command can be executed</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        /// <summary>
        /// Execute the encapsulated command
        /// </summary>
        /// <param name="parameter">the parameter that represents the execution method</param>
        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }

        #endregion ICommand Members

        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Destroys this instance.
        /// </summary>
        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        /// <summary>
        /// Defines if command can be executed (default behaviour)
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Always true</returns>
        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
    public class RelayCommand : ICommand
    {
        #region Fields

        /// <summary>
        /// Encapsulated the execute action
        /// </summary>
        private Action _execute;

        /// <summary>
        /// Encapsulated the representation for the validation of the execute method
        /// </summary>
        private Predicate<object> _canExecute;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RelayCommand class
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute)
            : this(execute, DefaultCanExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action execute, Predicate<object> canExecute)
        {
            this._execute = execute ?? throw new ArgumentNullException("execute");
            this._canExecute = canExecute ?? throw new ArgumentNullException("canExecute");
        }

        #endregion Constructors

        #region ICommand Members

        /// <summary>
        /// An event to raise when the CanExecute value is changed
        /// </summary>
        /// <remarks>
        /// Any subscription to this event will automatically subscribe to both 
        /// the local OnCanExecuteChanged method AND
        /// the CommandManager RequerySuggested event
        /// </remarks>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        /// <summary>
        /// An event to allow the CanExecuteChanged event to be raised manually
        /// </summary>
        private event EventHandler CanExecuteChangedInternal;

        /// <summary>
        /// Defines if command can be executed
        /// </summary>
        /// <param name="parameter">the parameter that represents the validation method</param>
        /// <returns>true if the command can be executed</returns>
        public bool CanExecute(object parameter)
        {
            return this._canExecute != null && this._canExecute(parameter);
        }

        /// <summary>
        /// Execute the encapsulated command
        /// </summary>
        /// <param name="parameter">the parameter that represents the execution method</param>
        public void Execute(object parameter)
        {
            this._execute();
        }

        #endregion ICommand Members

        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Destroys this instance.
        /// </summary>
        public void Destroy()
        {
            this._canExecute = _ => false;
            this._execute = null;
        }

        /// <summary>
        /// Defines if command can be executed (default behaviour)
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Always true</returns>
        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
