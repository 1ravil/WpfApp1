using System;
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
        private Студенты currentPartner = new Студенты();

        public bool check = false;

        public AddEditPage(Студенты SelectedPartner)
        {
            InitializeComponent();
            if (SelectedPartner != null)
            {
                check = true;
                currentPartner = SelectedPartner;
            }
            var currentKafedra = student_performanceEntities.GetContext().Кафедры.ToList();
            var ComboItem = currentKafedra.Select(p=> p.Код_Кафедры).ToList();
            PartnerTypeComboBox.Items.Clear();
            PartnerTypeComboBox.ItemsSource = ComboItem;
            


            DataContext = currentPartner;

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentPartner.Имя))
                errors.AppendLine("Укажите имя ученика");
            if (string.IsNullOrWhiteSpace(currentPartner.Фамилия))
                errors.AppendLine("Укажите фамилию ученика");
            if (string.IsNullOrWhiteSpace(currentPartner.Отчество))
                errors.AppendLine("Укажите отчество ученика");
            if (string.IsNullOrWhiteSpace(currentPartner.Код__кафедры.ToString()))
                errors.AppendLine("Укажите класс ученика");
            if (string.IsNullOrWhiteSpace(currentPartner.Год_рождения.ToString()))
                errors.AppendLine("Укажите дату рождения");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            currentPartner.Код__кафедры = PartnerTypeComboBox.SelectedIndex + 1;

            var allPartner = student_performanceEntities.GetContext().Студенты.ToList();
            allPartner = allPartner.Where(p => p.Имя == currentPartner.Имя).ToList();


            if (allPartner.Count == 0 || check == true)
            {
                if (currentPartner.Код_студента == 0)
                {
                    student_performanceEntities.GetContext().Студенты.Add(currentPartner);
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
            else
                MessageBox.Show("Такой студент уже сущесвтует!");

        }

    
    }
}
