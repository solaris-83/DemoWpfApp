using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWpfApp.Models;
using DemoWpfApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Messaging;
using MVVMNavigationModule.Abstractions;

namespace DemoWpfApp.ViewModels
{
    public partial class CustomersViewModel : ObservableObject, INavigatingFromAware, INavigatedToAware
    {
        private readonly ICustomerRepository _customerRepository;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RemoveCommand))]
        private Customer _selectedCustomer;

        [ObservableProperty]
        private string _userName;

        public CustomersViewModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ??
                                  throw new ArgumentNullException(nameof(customerRepository));
        }

        public IEnumerable<Customer> Customers => _customerRepository.Customers;

        [RelayCommand]
        private void Add()
        {
            var customer = new Customer();
            _customerRepository.Add(customer);
            SelectedCustomer = customer;
            OnPropertyChanged(nameof(Customers));
        }

        [RelayCommand(CanExecute = "HasSelectedCustomer")]
        private void Remove()
        {
            if (SelectedCustomer != null)
            {
                _customerRepository.Remove(SelectedCustomer);
                SelectedCustomer = null;
                OnPropertyChanged(nameof(Customers));
            }
        }

        private bool HasSelectedCustomer() => SelectedCustomer != null;

        [RelayCommand]
        private void Save()
        {
            _customerRepository.Commit();
        }

        [RelayCommand]
        private void Search(string textToSearch)
        {
            var coll = CollectionViewSource.GetDefaultView(Customers);
            if (!string.IsNullOrWhiteSpace(textToSearch))
                coll.Filter = c => ((Customer)c).Country.ToLower().Contains(textToSearch.ToLower());
            else
                coll.Filter = null;
        }

        public void OnNavigatingFrom()
        {
            // Unregisters the recipient from a message type
            // Pay attention: this is not necessarily always the best position in code. It's up to our needs
            WeakReferenceMessenger.Default.Unregister<LoggedInUserChangedMessage>(this);
        }

        public void OnNavigatedTo(object arg)
        {
            // Pay attention: this is not necessarily always the best position in code. It's up to our needs
            WeakReferenceMessenger.Default.Register<LoggedInUserChangedMessage>(this, (r, m) =>
            {
                // Handle the message here, with r being the recipient and m being the
                // input message. Using the recipient passed as input makes it so that
                // the lambda expression doesn't capture "this", improving performance.

                UserName = m.UserName;
            });
        }
    }
}
