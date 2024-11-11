using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Page
    {
        public List<Кафедры> currentKafedra = new List<Кафедры> { };
        int studentId;
        public StudentPage()
        {
            InitializeComponent();

            List<Студенты> currentPartners = student_performanceEntities.GetContext().Студенты.ToList();

            PartnerListView.ItemsSource = currentPartners;
            currentKafedra = student_performanceEntities.GetContext().Кафедры.ToList();
            var ComboItem = currentKafedra.Select(p => p.Название_кафедры).ToList();
            ComboItem.Insert(0, "Все");
            ComboFilter.Items.Clear();
            ComboFilter.ItemsSource = ComboItem;

            SortCombo.SelectedIndex = 0;
            ComboFilter.SelectedIndex = 0;
            Update();
        }
        public void Update()
        {
            var currentStudent = student_performanceEntities.GetContext().Студенты.ToList();
            if(SortCombo.SelectedIndex == 1)
            {
                currentStudent = currentStudent.OrderBy(p => p.Фамилия).ToList();
            }
            if (SortCombo.SelectedIndex == 2)
            {
                currentStudent = currentStudent.OrderByDescending(p => p.Фамилия).ToList();
            }
            //if (SortCombo.SelectedIndex == 3)
            //{
            //    currentStudent = currentStudent.OrderBy(p => p.bith).ToList();
            //}
            //if (SortCombo.SelectedIndex == 4)
            //{
            //    currentStudent = currentStudent.OrderByDescending(p => p.bith).ToList();
            //}
            if(ComboFilter.SelectedIndex!=0 && ComboFilter.SelectedIndex!=-1)
            {
                var Currentcombo = currentKafedra.Where(p => p.Название_кафедры == ComboFilter.SelectedItem.ToString()).Select(p=> p.Код_Кафедры).ToList();
                currentStudent = currentStudent.Where(p=> p.Код__кафедры == Currentcombo.First()).ToList();
            }
            currentStudent = currentStudent.Where(p => p.Фамилия.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Имя.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Отчество.ToLower().Contains(TBoxSearch.Text.ToLower()) 
            || p.Телефон.Replace("+", "").Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").ToLower().Contains(TBoxSearch.Text.Replace("+", "").Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").ToLower())).ToList();

            PartnerListView.ItemsSource = currentStudent;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Студенты));
            //var student = (Студенты)(sender as Button).DataContext;
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Студенты));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        

        

        private void PartnerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PartnerListView.SelectedIndex != -1)
            {
                var selectedStudent = PartnerListView.SelectedItem as Студенты;
                if (selectedStudent != null)
                {
                    studentId = selectedStudent.Код_студента;  // Предполагаем, что у вас есть свойство Id в классе Студенты
                }
            }
        }
    }
}
