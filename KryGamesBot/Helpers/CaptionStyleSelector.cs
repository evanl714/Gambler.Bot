using DevExpress.Mvvm;
using DevExpress.Xpf.Docking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KryGamesBot.Helpers
{
    public class CaptionStyleSelector : StyleSelector
    {
        public Style AddNewTabStyle { get; set; }
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ContentItem && ((ContentItem)item).Content is AddNewTabViewModel)
                return AddNewTabStyle;
            return base.SelectStyle(item, container);
        }
    }
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Documents = new ObservableCollection<DocumentViewModel>();
            Documents.Add(new DocumentViewModel() { DisplayName = "Select a site ", Content = new InstanceControl() });
            Documents.Add(new AddNewTabViewModel());
            CloseCommand = new DelegateCommand<DocumentViewModel>(Close);
            AddNewCommand = new DelegateCommand(AddNew);
        }
        public ObservableCollection<DocumentViewModel> Documents
        {
            get { return GetProperty(() => Documents); }
            private set { SetProperty(() => Documents, value); }
        }
        public ICommand CloseCommand { get; private set; }
        public ICommand AddNewCommand { get; private set; }
        public void Close(DocumentViewModel viewModel)
        {
            Documents.Remove(viewModel);
        }
        public void AddNew()
        {
            Documents.Insert(Documents.Count - 1, new DocumentViewModel() { DisplayName = "Select a site ", Content=new InstanceControl()}) ;
        }
        int documentCount;
    }

    public class DocumentViewModel : ViewModelBase
    {
        public DocumentViewModel()
        {
            AllowActivate = true;
        }
        public string DisplayName
        {
            get { return GetProperty(() => DisplayName); }
            set { SetProperty(() => DisplayName, value); }
        }
        public object Content
        {
            get { return GetProperty(() => Content); }
            set { SetProperty(() => Content, value); }
        }
        public bool AllowActivate
        {
            get { return GetProperty(() => AllowActivate); }
            set { SetProperty(() => AllowActivate, value); }
        }
    }

    public class AddNewTabViewModel : DocumentViewModel
    {
        public AddNewTabViewModel()
        {
            AllowActivate = false;
        }
    }
}
