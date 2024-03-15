using DataStorageWPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp
{
    // Class representing a Person with Id, Name, and Age properties
    public class Person : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private int _age;
        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        // Event to notify UI of property changes
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class MainWindow : Window
    {
        private readonly Person[] _people = new Person[100]; // Array to store Person objects
        private int _count = 0; // Counter to keep track of number of records

        public MainWindow()
        {
            InitializeComponent();
        }

        // Button click event to add a new Person record
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_count < _people.Length)
            {
                // Create a new Person object and add it to the array
                var person = new Person { Id = int.Parse(IdTextBox.Text), Name = NameTextBox.Text, Age = int.Parse(AgeTextBox.Text) };
                _people[_count++] = person;
            }
            else
            {
                MessageBox.Show("Storage is full.");
            }
        }

        // Button click event to display all Person records
        private void DisplayButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayListBox.Items.Clear();
            foreach (var person in _people.Take(_count))
            {
                // Display each Person record in the ListBox
                DisplayListBox.Items.Add($"ID: {person.Id}, Name: {person.Name}, Age: {person.Age}");
            }
        }

        // Button click event to sort Person records by Age
        private void SortByAgeButton_Click(object sender, RoutedEventArgs e)
        {
            // Sort the array of Person objects by Age using Array.Sort() method
            Array.Sort(_people, 0, _count, Comparer<Person>.Create((x, y) => x.Age.CompareTo(y.Age)));
            DisplayButton_Click(sender, e); // Display sorted records
        }

        // Button click event to sort Person records by Name
        private void SortByNameButton_Click(object sender, RoutedEventArgs e)
        {
            // Sort the array of Person objects by Name using Array.Sort() method
            Array.Sort(_people, 0, _count, Comparer<Person>.Create((x, y) => string.Compare(x.Name, y.Name)));
            DisplayButton_Click(sender, e); // Display sorted records
        }

        // Button click event to search Person records by Age
        private void SearchByAgeButton_Click(object sender, RoutedEventArgs e)
        {
            int ageToSearch = int.Parse(SearchAgeTextBox.Text);
            // Filter Person records based on Age and display the result
            DisplaySearchResult(_people.Take(_count).Where(p => p.Age == ageToSearch).ToList());
        }

        // Button click event to search Person records by Name
        private void SearchByNameButton_Click(object sender, RoutedEventArgs e)
        {
            string nameToSearch = SearchNameTextBox.Text;
            // Filter Person records based on Name and display the result
            DisplaySearchResult(_people.Take(_count).Where(p => p.Name.Equals(nameToSearch, StringComparison.OrdinalIgnoreCase)).ToList());
        }

        // Helper method to display search result in the ListBox
        private void DisplaySearchResult(System.Collections.Generic.List<Person> result)
        {
            DisplayListBox.Items.Clear();
            foreach (var person in result)
            {
                DisplayListBox.Items.Add($"ID: {person.Id}, Name: {person.Name}, Age: {person.Age}");
            }
        }

        // Button click event to remove a Person record by Id
        private void RemoveByIdButton_Click(object sender, RoutedEventArgs e)
        {
            int idToRemove = int.Parse(RemoveIdTextBox.Text);
            // Find index of the Person record with the given Id
            int index = Array.FindIndex(_people, 0, _count, p => p.Id == idToRemove);
            if (index != -1)
            {
                // Shift elements to the left to remove the record
                Array.Copy(_people, index + 1, _people, index, _count - index - 1);
                _count--; // Decrement record count
                MessageBox.Show("Record removed successfully.");
            }
            else
            {
                MessageBox.Show("Record not found.");
            }
        }
    }
}