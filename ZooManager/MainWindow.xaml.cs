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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq.Expressions;

namespace ZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["ZooManager.Properties.Settings.myDBConnectionString"].ConnectionString;
            

            sqlConnection = new SqlConnection(connectionString);

            ShowZoos();

            ShowAnimals();


        }

        private void ShowZoos()
        {
            try
            {
                string query = "select * from Zoo";


                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);

                using (adapter)
                {
                    DataTable zooTable = new DataTable();

                    adapter.Fill(zooTable);

                    ListZoos.DisplayMemberPath = "Location";
                    ListZoos.SelectedValuePath = "Id";
                    ListZoos.ItemsSource = zooTable.DefaultView;
                }

            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }
                


        }


        private void ShowAssociatedAnimals()
        {
            try
            {
                string query = "select * from Animal a inner join ZooAnimal za on a.Id = za.AnimalId where za.ZooId = @ZooId";


                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", ListZoos.SelectedValue);

                    DataTable animalTable = new DataTable();

                    adapter.Fill(animalTable);

                    ListAssociatedAnimals.DisplayMemberPath = "Name";
                    ListAssociatedAnimals.SelectedValuePath = "Id";
                    ListAssociatedAnimals.ItemsSource = animalTable.DefaultView;
                }

            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }



        }



        private void ShowAnimals()
        {

            try
            {
                string query = "Select * from Animal";

                SqlDataAdapter  adapter = new SqlDataAdapter(query, sqlConnection);

                using(adapter)
                {
                    DataTable animalsDT = new DataTable();
                    adapter.Fill(animalsDT);


                    AnimalsList.DisplayMemberPath = "Name";
                    AnimalsList.SelectedValuePath = "Id";
                    AnimalsList.ItemsSource = animalsDT.DefaultView;

                }





            }catch(Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }


        private void ListZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          ShowAssociatedAnimals();
            showSelectedZooInTextBox();
        }


       private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                string query = "delete from Zoo where id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
                

            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());

            }
            finally
            {
                sqlConnection.Close();
            }
 

            ShowZoos();
        }


        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string query = "insert into Zoo values (@Location)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", myTxtBox.Text);
                myTxtBox.Clear();
                sqlCommand.ExecuteScalar();


            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());

            }
            finally
            {
                sqlConnection.Close();
            }


            ShowZoos();
        }


        private void addAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string query = "insert into ZooAnimal values (@ZooId, @AnimalId)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", AnimalsList.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@ZooId", ListZoos.SelectedValue);
                sqlCommand.ExecuteScalar();


            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());

            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();
            }


            ShowZoos();

        }



        private void deleteAnimalFromAnimals_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                string query = "delete from Animal where id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", AnimalsList.SelectedValue);
                sqlCommand.ExecuteScalar();


            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());

            }
            finally
            {
                sqlConnection.Close();
            }


            ShowAnimals();
        }


        private void removeAnimal_Click(object obj, RoutedEventArgs e)
        {
            try
            {

                string query = "delete from ZooAnimal where ZooId = @ZooId and AnimalId = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListAssociatedAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@ZooId", ListZoos.SelectedValue);
                sqlCommand.ExecuteScalar();


            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());

            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();
            }


            ShowZoos();
        }

        private void addAnimalZoo_Click(object obj, RoutedEventArgs e)
        {

            try
            {
                string query = "insert into Animal values (@Name)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", myTxtBox.Text);
                sqlCommand.ExecuteScalar();
                myTxtBox.Clear();

            }catch(Exception er)
            {
                MessageBox.Show(er.ToString());
            }
            finally
            {
                ShowAnimals();
                ShowAssociatedAnimals();
                sqlConnection.Close();

            }


        }


        private void showSelectedZooInTextBox()
        {
            try
            {
                string query = "select Location from zoo where Id = @ZooId";


                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", ListZoos.SelectedValue);

                    DataTable zooDataTable = new DataTable();

                    adapter.Fill(zooDataTable);

                    myTxtBox.Text = zooDataTable.Rows[0]["Location"].ToString(); 


                }

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());
            }   

        }


        private void showSelectedAnimalInTextBox()
        {
            try
            {
                string query = "select Name from Animal where Id = @AnimalId";


                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", AnimalsList.SelectedValue);

                    DataTable animalDataTable = new DataTable();

                    adapter.Fill(animalDataTable);

                    myTxtBox.Text = animalDataTable.Rows[0]["Name"].ToString();


                }

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());
            }

        }

        private void AnimalsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showSelectedAnimalInTextBox();
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string query = "update Zoo Set Location = @Location where Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", myTxtBox.Text);
                sqlCommand.Parameters.AddWithValue("@ZooId", ListZoos.SelectedValue);
                sqlCommand.ExecuteScalar();


            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());

            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            } 

        }


        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string query = "update Animal Set Name = @Name where Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", myTxtBox.Text);
                sqlCommand.Parameters.AddWithValue("@AnimalId", AnimalsList.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString());

            }
            finally
            {
                sqlConnection.Close();
               ShowAnimals();
            }

        }
    }
}
