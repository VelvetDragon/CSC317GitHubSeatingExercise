using System.Collections.Specialized;
using System.Reflection.Metadata;

namespace CSC317GitHubSeatingExercise
{
    public class SeatingUnit
    {
        public string Name { get; set; }
        public bool Reserved { get; set; }

        public SeatingUnit(string name, bool reserved = false)
        {
            Name = name;
            Reserved = reserved;
        }

    }

    public partial class MainPage : ContentPage
    {
        SeatingUnit[,] seatingChart = new SeatingUnit[5, 10];

        public MainPage()
        {
            InitializeComponent();
            GenerateSeatingNames();
            RefreshSeating();
        }

        private async void ButtonReserveSeat(object sender, EventArgs e)
        {
            var seat = await DisplayPromptAsync("Enter Seat Number", "Enter seat number: ");

            if (seat != null)
            {
                for (int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == seat)
                        {
                            if (seatingChart[i, j].Reserved == true)
                            {
                                await DisplayAlert("Error", "This seat is already reserved.", "Ok");
                                return;
                            }

                            seatingChart[i, j].Reserved = true;
                            await DisplayAlert("Successfully Reserved", "Your seat was reserved successfully!", "Ok");
                            RefreshSeating();
                            return;
                        }
                    }
                }

                await DisplayAlert("Error", "Seat was not found.", "Ok");
            }
        }

        private void GenerateSeatingNames()
        {
            List<string> letters = new List<string>();
            for (char c = 'A'; c <= 'Z'; c++)
            {
                letters.Add(c.ToString());
            }

            int letterIndex = 0;

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    seatingChart[row, column] = new SeatingUnit(letters[letterIndex] + (column + 1).ToString());
                }

                letterIndex++;
            }
        }

        private void RefreshSeating()
        {
            grdSeatingView.RowDefinitions.Clear();
            grdSeatingView.ColumnDefinitions.Clear();
            grdSeatingView.Children.Clear();

            for (int row = 0; row < seatingChart.GetLength(0); row++)
            {
                var grdRow = new RowDefinition();
                grdRow.Height = 50;

                grdSeatingView.RowDefinitions.Add(grdRow);

                for (int column = 0; column < seatingChart.GetLength(1); column++)
                {
                    var grdColumn = new ColumnDefinition();
                    grdColumn.Width = 50;

                    grdSeatingView.ColumnDefinitions.Add(grdColumn);

                    var text = seatingChart[row, column].Name;

                    var seatLabel = new Label();
                    seatLabel.Text = text;
                    seatLabel.HorizontalOptions = LayoutOptions.Center;
                    seatLabel.VerticalOptions = LayoutOptions.Center;
                    seatLabel.BackgroundColor = Color.Parse("#333388");
                    seatLabel.Padding = 10;

                    if (seatingChart[row, column].Reserved == true)
                    {
                        //Change the color of this seat to represent its reserved.
                        seatLabel.BackgroundColor = Color.Parse("#883333");

                    }

                    Grid.SetRow(seatLabel, row);
                    Grid.SetColumn(seatLabel, column);
                    grdSeatingView.Children.Add(seatLabel);

                }
            }
        }

      //Suwan Aryal / Chetanchal Saud
     private async void ButtonReserveRange(object sender, EventArgs e)
     {
            //-- suwan
            var seat = await DisplayPromptAsync("Enter the range of seats", "Enter your range (e.g., A5:A8): ");

            if (seat != null)
            {
                
                string[] seatRange = seat.Split(':'); // splitting the input string into an array to separate the start and end seats.

                
                if (seatRange.Length == 2) //checking if the input contains exactly two elements (start and end seat).
                {
                    
                    string seatStart = seatRange[0];
                    string seatEnd = seatRange[1];
                    
                    int startRow = -1, startColumn = -1, endRow = -1, endColumn = -1; //initializing row and column indices for the start and end seats to -1 (indicating not found).
                    
                    for (int i = 0; i < seatingChart.GetLength(0); i++) // iterating through the seating chart to locate the indices of the start and end seats.
                    {
                        for (int j = 0; j < seatingChart.GetLength(1); j++) 
                        {
                            
                            if (seatingChart[i, j].Name == seatStart)
                            {                               
                                startRow = i;
                                startColumn = j;
                            }
                            
                            if (seatingChart[i, j].Name == seatEnd)
                            {                                
                                endRow = i;
                                endColumn = j;
                            }
                        }
                    }
                    
                    if (startRow == endRow && startRow != -1 && startColumn != -1 && endColumn != -1) // checking to ensure the range lies on the same row (Logic is --> if start row and end row are same the seat range lies in same row so a valid range to reserve
                    // For B1:B7 it is the second row (index = 1) so the startRow and endRow will be the same (1)
                    {
                        
                        bool seatsAvailable = true;
                        
                        for (int j = startColumn; j <= endColumn; j++)
                        {
                            if (seatingChart[startRow, j].Reserved == true)
                            {
                                seatsAvailable = false;
                                break; 
                            }
                        }
            
     }  

        //Bibas Kandel
        private async void ButtonCancelReservation(object sender, EventArgs e)
        {
        var seat = await DisplayPromptAsync("Enter Seat Number", "Enter seat number: ");

            if(seat != null)
            {
                for(int i = 0; i < seatingChart.GetLength(0); i++)
                {
                    for (int j = 0; j < seatingChart.GetLength(1); j++)
                    {
                        if (seatingChart[i, j].Name == seat)
                        {
                            seatingChart[i, j].Reserved = false;
                            await DisplayAlert("Successfully Cancelled Reservation", "Your reservation was cancelled successfully!", "Ok");
                            RefreshSeating();
                            return;
                        }
                    }
                }

                await DisplayAlert ("Error", "Seat was not found.", "Ok");
            }

        }

      //Kadin Dengler / Chetanchal Saud
     private async void ButtonCancelReservationRange(object sender, EventArgs e)
     {
         var startSeat = await DisplayPromptAsync("Cancel Reservation Range", "Enter starting seat number: ");
         var endSeat = await DisplayPromptAsync("Cancel Reservation Range", "Enter ending seat number: ");

         if (startSeat != null && endSeat != null)
         {
             bool rangeFound = false;
             for (int i = 0; i < seatingChart.GetLength(0); i++)
             {
                 for (int j = 0; j < seatingChart.GetLength(1); j++)
                 {
                     if (seatingChart[i, j].Name == startSeat)
                     {
                         rangeFound = true;
                     }

                     if (rangeFound)
                     {
                         seatingChart[i, j].Reserved = false;

                         if (seatingChart[i, j].Name == endSeat)
                         {
                             await DisplayAlert("Success", "The reservation range was successfully canceled!", "Ok");
                             RefreshSeating();
                             return;
                         }
                     }
                 }
             }
         }
     }  


        //Gunjan Sah
        private async void ButtonResetSeatingChart(object sender, EventArgs e)
        {
        for(int i = 0; i < seatingChart.GetLength(0); i++)
            {
                for (int j = 0; j < seatingChart.GetLength(1); j++)
                {
                    if (seatingChart[i, j].Reserved == true)
                    { 
                        seatingChart[i, j].Reserved = false;
                        
                    }
                }
            }
            await DisplayAlert("All Reservations Cleared",
                "All seat reservations were cleared successfully !!!", "Ok");
            RefreshSeating();
            return;


        }
    }

}
