using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LSystems
{
    /// <summary>
    /// Base class for commands implementation
    /// </summary>
    abstract class BaseCommand : ICommand
    {
        protected readonly MainWindowViewModel _viewModel;

        public bool Disabled { get; set; }

        public BaseCommand(MainWindowViewModel viewModel, bool disabled = false)
        {
            _viewModel = viewModel;
            Disabled = disabled;
        }

        public abstract void Execute(object parameter);

        public bool CanExecute(object parameter)
        {
            return !Disabled;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
    
    /// <summary>
    /// Command handler class for Generate L-System button
    /// </summary>
    class GenerateCommand : BaseCommand
    {
        public GenerateCommand(MainWindowViewModel viewModel, bool disabled = false) : base(viewModel, disabled) { }

        public override void Execute(object parameter)
        {
            _viewModel.GenerateImage();
        }
    }

    /// <summary>
    /// Command handler class for Save Image button
    /// </summary>
    class SaveImageCommand : BaseCommand
    {
        public SaveImageCommand(MainWindowViewModel viewModel, bool disabled = false) : base(viewModel, disabled) { }

        public override void Execute(object parameter)
        {
            _viewModel.SaveImage();
        }
    }

    /// <summary>
    /// Command handler for close menu item
    /// </summary>
    class CloseCommand : BaseCommand
    {
        public CloseCommand(MainWindowViewModel viewModel, bool disabled = false) : base(viewModel, disabled) { }

        public override void Execute(object parameter)
        {
            _viewModel.Close();
        }
    }

    /// <summary>
    /// Command handler for new random seed
    /// </summary>
    class RandomSeedCommand : BaseCommand
    {
        public RandomSeedCommand(MainWindowViewModel viewModel, bool disabled = false) : base(viewModel, disabled) { }

        public override void Execute(object parameter)
        {
            _viewModel.RandomizeSeed();
        }
    }

    /// <summary>
    /// Command handler for saving definition of l-system
    /// </summary>
    class SaveDefinitionCommand : BaseCommand
    {
        public SaveDefinitionCommand(MainWindowViewModel viewModel, bool disabled = false) : base(viewModel, disabled) { }

        public override void Execute(object parameter)
        {
            _viewModel.SaveDefinition();
        }
    }

    /// <summary>
    /// Command handler for loading definition of l-system
    /// </summary>
    class LoadDefinitionCommand : BaseCommand
    {
        public LoadDefinitionCommand(MainWindowViewModel viewModel, bool disabled = false) : base(viewModel, disabled) { }

        public override void Execute(object parameter)
        {
            _viewModel.LoadDefinition();
        }
    }
}
