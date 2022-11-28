using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TobaccoCompanyWPF.Models;

namespace TobaccoCompanyWPF.ViewModels.MVVM
{
    public abstract class BaseViewModel : PropertyValidateModelViaNotify, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool DialogHostLoadingIsOpen { get; set; } = false;

        public bool DialogHostQuestionIsOpen { get; set; } = false;

        protected string BackGroundFileName = "background.png";

        public string BackGroundImage => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, @$"Public\Resources\{BackGroundFileName}");

        public string QuestionText { get; set; } = "";

        public ICommand? DialogHostQuestionYesAnswerCommand { get; set; }

        public ICommand? DialogHostQuestionNoAnswerCommand { get; set; }

        protected void AskQuestiion(string questionText, Action<object?> actionOnYesAnswer, Action<object?>? actionOnNoAnswer = null)
        {
            this.QuestionText = questionText;
            actionOnYesAnswer += _ => this.DialogHostQuestionIsOpen = false;
            actionOnNoAnswer += _ => this.DialogHostQuestionIsOpen = false;
            this.DialogHostQuestionYesAnswerCommand = new Command(actionOnYesAnswer);
            this.DialogHostQuestionNoAnswerCommand = new Command(actionOnNoAnswer);
            this.DialogHostQuestionIsOpen = true;
        }

        protected void AskQuestiionAsync(string questionText, Func<Task> actionOnYesAnswer, Func<Task>? actionOnNoAnswer = null)
        {
            this.QuestionText = questionText;
            var task = new Func<Func<Task>?, Task>(async (func) =>
            {
                this.DialogHostQuestionIsOpen = false;
                if (func is not null)
                {
                    this.DialogHostLoadingIsOpen = true;
                    await func();
                    this.DialogHostLoadingIsOpen = false;
                }
            });
            this.DialogHostQuestionYesAnswerCommand = new AsyncCommand(() => task(actionOnYesAnswer));
            this.DialogHostQuestionNoAnswerCommand = new AsyncCommand(() => task(actionOnNoAnswer));
            this.DialogHostQuestionIsOpen = true;
        }

        protected async Task DoActionWithLoadingAsync(Action action)
        {
            this.DialogHostLoadingIsOpen = true;
            await Task.Run(action);
            this.DialogHostLoadingIsOpen = false;
        }

        protected async Task<T> DoActionWithLoadingAsync<T>(Func<T> func)
        {
            this.DialogHostLoadingIsOpen = true;
            var result = await Task.Run(func);
            this.DialogHostLoadingIsOpen = false;
            return result;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
