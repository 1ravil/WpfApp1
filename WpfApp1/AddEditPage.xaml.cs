using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Студенты currentStudent = new Студенты();

        public AddEditPage(Студенты SelectedStudent)
        {

            InitializeComponent();
            
            if(SelectedStudent !=  null)
            {
                currentStudent = SelectedStudent;
                KafedraComboBox.SelectedIndex = (int)(currentStudent.Код__кафедры - 101);
                //BirthdayStudent.Text = currentStudent.Год_рождения.ToString();
                DeleteBtn.Visibility = Visibility.Visible;
            }
            else
            {
                KafedraComboBox.SelectedIndex = 0;
                DeleteBtn.Visibility = Visibility.Hidden;
            }

            DataContext = currentStudent;

        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentStudent.Имя))
                errors.AppendLine("Укажите имя студента");
            if (string.IsNullOrWhiteSpace(currentStudent.Фамилия))
                errors.AppendLine("Укажите фамилию студента");
            if (string.IsNullOrWhiteSpace(currentStudent.Отчество))
                errors.AppendLine("Укажите отчество студента");
            //if (string.IsNullOrWhiteSpace(currentStudent.Адрес))
            //errors.AppendLine("Укажите адрес студента");
            //if (string.IsNullOrWhiteSpace(currentStudent.Пол))
            //    errors.AppendLine("Укажите пол студента");
            //if (string.IsNullOrWhiteSpace(currentStudent.Город))
            //    errors.AppendLine("Укажите город студента");
            //if (BirthdayStudent.Text == "")
            //    errors.AppendLine("Укажите дату рождения");
            if (string.IsNullOrWhiteSpace(currentStudent.Телефон))
                errors.AppendLine("Укажите телефон студента");
            if (string.IsNullOrWhiteSpace(currentStudent.Специальность))
                errors.AppendLine("Укажите телефон студента");
            if (string.IsNullOrWhiteSpace(currentStudent.Группа))
                errors.AppendLine("Укажите телефон студента");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            currentStudent.Код__кафедры = KafedraComboBox.SelectedIndex + 101;

           




            if (currentStudent.Код_студента == 0)
            {
                student_performanceEntities.GetContext().Студенты.Add(currentStudent);
            }

            try
            {
                student_performanceEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                currentStudent.Фото = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentStudent = (sender as Button).DataContext as Студенты;

            var currentStudentsList = student_performanceEntities.GetContext().Ведомости_успеваемости.ToList();

            currentStudentsList = currentStudentsList.Where(p => p.Код_студента == currentStudent.Код_студента).ToList();

            if(currentStudentsList.Count != 0)
            {
                MessageBox.Show("Невозможно выполнить удаление!");
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        student_performanceEntities.GetContext().Студенты.Remove(currentStudent);
                        student_performanceEntities.GetContext().SaveChanges();
                        Manager.MainFrame.GoBack();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }

        }
    }
}
