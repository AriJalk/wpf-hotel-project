using HotelProject.Model.BaseClasses;
using HotelProject.Model.DbClasses;
using IronPdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelProject.ViewModel.Helpers.Pdf
{
    /// <summary>
    /// Class for generating reports
    /// First an HTML code is generated from template
    /// than it is passed to IronPDF for conversion to PDF 
    /// </summary>
    public static class HtmlReports
    {

        static string LogoUri;

        static HtmlReports()
        {
            var pngBinaryData = File.ReadAllBytes(ObjectFileHelper.GetFullPath(@"\Resources\Images\HotelLogo.png"));
            LogoUri = @"data:image/png;base64," + Convert.ToBase64String(pngBinaryData);
        }
        /// <summary>
        /// Generates A Reservation Document for the client
        /// </summary>
        /// <param name="reservation"></param>
        public static void ReservationTicket(RoomReservation reservation, User user)
        {
            string pension = string.Empty;
            decimal total = 0;
            //Check pension type
            if (reservation.TransactionList.Count > 0)
            {
                if (reservation.TransactionList[0].TransactionPartList.Count > 0)
                {
                    if (reservation.TransactionList[0].TransactionPartList[0].Service.Name.Contains("Half"))
                        pension = "Half Pension";
                    else if (reservation.TransactionList[0].TransactionPartList[0].Service.Name.Contains("Full"))
                        pension = "Full Pension";
                }
                total = reservation.TransactionList[0].ToPayAmount;
            }
            string template =
                 "<!DOCTYPE html>" +
                 "<html>" +
                 "<head>" +
                 "<title>[Title] </title>" +
                 "<link rel =\"stylesheet\" type = \"text/css\" href =\"style.css\"/>" +
                 "<style>" +
                 "table, th, td {" +
                 "border: 1px solid black;" +
                "border-collapse: seperate;}" +
                "</style>" +
                 "</head>" +
                 "<body>" +
                 $"<img src={LogoUri} Width=300px Height=auto>" +
                 "<div>" +
                 "<h4>" +
                 "Reservation:" + reservation.RoomReservationId.ToString() + "<br /><br />" +
                 "Customer Name: " + reservation.Customer.FName + " " + reservation.Customer.LName + "<br /><br />" +
                 "Phone Number: " + reservation.Customer.PhoneNumber + "</h4>" +
                 "<h4>" +
                 "Room " + reservation.Room.ElementNumber.ToString() + "<br /><br />" +
                 "Floor " + reservation.Room.Floor.ElementNumber.ToString() + "<br /><br />" +
                 "# People: " + reservation.PeopleCount.ToString() + "</br></br>" +
                 "Check-In: " + reservation.StartTime.ToString() + " <br /><br />" +
                 "Check-Out: " + reservation.EndTime.ToString() + " <br /><br />" +
                 $"Pension: {pension}<br/><br/>" +
                 "<table><tr><th>Service</th><th>Price</th></tr>";
            foreach(Transaction transaction in reservation.TransactionList)
            {
                foreach(TransactionPart part in transaction.TransactionPartList)
                {
                    template += $"<tr><td>{part.Service.Name}</td><td>{part.Price.ToString("C", CultureInfo.CurrentCulture)}</td></tr>";
                }
            }

            template += "</table></br></br>Total: " + total.ToString("C", CultureInfo.CurrentCulture) + "<br/><br/>" +
            "</h4>";
            HTMLStringToPdf(template, $"Reservation{reservation.RoomReservationId}", user);
        }

        /// <summary>
        /// Generates a summary of all reservations
        /// </summary>
        /// <param name="reservationList"></param>
        public static void AllReservations(List<RoomReservation> reservationList, User user, DateTime startDate, DateTime endDate)
        {
            string template =
                "<!DOCTYPE html>" +
                "<html>" +
                "<head>" +
                "<title>All Reservations Report </title>" +
                "<link rel =\"stylesheet\" type = \"text/css\" href =\"style.css\"/>" +
                "<style>" +
                "table, th, td {" +
                "border: 1px solid black;" +
                "border-collapse: seperate;}" +
                "</style>" +
                "</head>" +
                "<body>" +
                $"<img src={LogoUri} Width=300px Height=auto>" +
                "<h3>All Reservations Report</br>" +
                $"{startDate.ToString("dd/MM/yyyy")} - {endDate.ToString("dd/MM/yyyy")}</h3>" +
                "<div>" +
                "<table>" +
                "<tr>" +
                "<th> Reservation #</th>" +
                "<th> Customer Name </th>" +
                "<th> Phone Number </th>" +
                "<th> Room </th>" +
                "<th> Floor </th>" +
                "<th> Check - In - Time </th>" +
                "<th> Check - Out - Time </th>" +
                "<th>Order Total</th></tr>";
            //Loop through all reservations
            string table = string.Empty;
            decimal allTotal = 0;
            int[] reservationsPerMonth = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            foreach (var reservation in reservationList)
            {
                decimal total = 0;
                //Loop through transactions in reservation
                foreach (var transaction in reservation.TransactionList)
                    total += transaction.ToPayAmount;
                allTotal += total;
                //Add fields to table
                table +=
                    "<tr>" +
                    "<td>" + reservation.RoomReservationId.ToString() + "</td>" +
                    "<td>" + reservation.Customer.FName + " " + reservation.Customer.LName + "</td>" +
                    "<td>" + reservation.Customer.PhoneNumber + "</td>" +
                    "<td>" + reservation.Room.ElementNumber.ToString() + "</td>" +
                    "<td>" + reservation.Room.Floor.ElementNumber.ToString() + "</td>" +
                    "<td>" + reservation.StartTime.ToString() + "</td>" +
                    "<td>" + reservation.EndTime.ToString() + "</td>" +
                    "<td>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td>" +
                    "</tr>";
                //Increment month
                reservationsPerMonth[reservation.StartTime.Month - 1]++;
            }
            template += table;
            string templateEnd =
                "</table>" +
                "<h3> Total=" + allTotal.ToString("C", CultureInfo.CurrentCulture) + "</h2>";
            template += templateEnd;
            template += BarChart(new List<string>(months), new List<int>(reservationsPerMonth), "Reservations per month", "Month", "Reservations", 0, false);
            HTMLStringToPdf(template, $"AllReservationsReport_{DateTime.Now.Year}{DateTime.Now.Month.ToString().PadLeft(2, '0')}{DateTime.Now.Day.ToString().PadLeft(2, '0')}", user);
        }

        public static void AllCustomersReport(List<Customer> customerList, User user)
        {
            string template =
                "<!DOCTYPE html>" +
                "<html>" +
                "<head>" +
                "<title>All Customers Report </title>" +
                "<link rel =\"stylesheet\" type = \"text/css\" href =\"style.css\"/>" +
                "<style>" +
                "table, th, td {" +
                "border: 1px solid black;" +
                "border-collapse: seperate;}" +
                "</style>" +
                "</head>" +
                "<body>" +
                $"<img src={LogoUri} Width=300px Height=auto>" +
                "<h3>All Customers Report</h3>" +
                "<div>" +
                "<table>" +
                "<tr>" +
                "<th> Customer #</th>" +
                "<th> Customer Name </th>" +
                "<th> Phone Number </th>" +
                "<th> # of reservations </th>+" +
                "<th>Total Payed</th></tr>";
            //Loop through all customers
            string table = string.Empty;
            foreach (var customer in customerList)
            {
                decimal total = 0;
                int count = 0;
                foreach (RoomReservation reservation in customer.RoomReservationList)
                {
                    foreach (Transaction transaction in reservation.TransactionList)
                    {
                        total += transaction.ToPayAmount;
                        count++;
                    }
                }
                //Add fields to table
                table +=
                    "<tr>" +
                    "<td>" + customer.CustomerId.ToString() + "</td>" +
                    "<td>" + customer.FName + " " + customer.LName + "</td>" +
                    "<td>" + customer.PhoneNumber + "</td>" +
                    "<td>" + count + "</td>" +
                    "<td>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td>" +
                    "</tr>";
            }
            template += table;
            string templateEnd =
                "</table>";
            template += templateEnd;
            HTMLStringToPdf(template, $"AllCustomersReport_{DateTime.Now.Year}{DateTime.Now.Month.ToString().PadLeft(2, '0')}{DateTime.Now.Day.ToString().PadLeft(2, '0')}", user);
        }

        public static void AllRoomsReport(List<Room> roomList, User user, DateTime startDate, DateTime endDate)
        {
            string template =
               "<!DOCTYPE html>" +
               "<html>" +
               "<head>" +
               "<title>All Rooms Report </title>" +
               "<link rel =\"stylesheet\" type = \"text/css\" href =\"style.css\"/>" +
               "<style>" +
               "table, th, td {" +
               "border: 1px solid black;" +
               "border-collapse: seperate;}" +
               "</style>" +
               "</head>" +
               "<body>" +
               $"<img src={LogoUri} Width=300px Height=auto>" +
               "<h3>All Rooms Report</br>" +
               $"{startDate.ToString("dd/MM/yyyy")} - {endDate.ToString("dd/MM/yyyy")}</h3>" +
               "<div>" +
               "<table>" +
               "<tr>" +
               "<th>Room #</th>" +
               "<th>Floor #</th>" +
               "<th>Type</th>" +
               "<th>Reservations</th>" +
               "<th>Total Revenue</th></tr>";
            //Loop through all rooms
            string table = string.Empty;
            decimal avg = 0;
            decimal avg_counter = 0;
            foreach (Room room in roomList)
            {
                decimal total = 0;
                foreach (RoomReservation reservation in room.RoomReservationList)
                {
                    foreach (Transaction transaction in reservation.TransactionList)
                    {
                        total += transaction.ToPayAmount;
                    }
                }
                table +=
                    "<tr>" +
                    "<td>" + room.ElementNumber + "</td>" +
                    "<td>" + room.Floor.ElementNumber + "</td>" +
                    "<td>" + room.RoomType.Name + "</td>" +
                    "<td>" + room.RoomReservationList.Count + "</td>" +
                    "<td>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td>" +
                    "</tr>";
                if (total > 0)
                {
                    avg += total;
                    avg_counter++;
                }
            }
            //Add fields to table
            template += table;
            string templateEnd =
                "</table>" +
                "<h4> Avarage Reservation Total for Rooms - " + (avg / avg_counter).ToString("C", CultureInfo.CurrentCulture) + "</h4>";
            template += templateEnd;
            HTMLStringToPdf(template, $"AllRoomsReport_{DateTime.Now.Year}{DateTime.Now.Month.ToString().PadLeft(2, '0')}{DateTime.Now.Day.ToString().PadLeft(2, '0')}", user);
        }

        public static void AllFloorsReport(List<Floor> floorList, User user, DateTime startDate, DateTime endDate)
        {
            string template =
               "<!DOCTYPE html>" +
               "<html>" +
               "<head>" +
               "<title>All Floors Report </title>" +
               "<link rel =\"stylesheet\" type = \"text/css\" href =\"style.css\"/>" +
               "<style>" +
               "h1{color:cornflowerblue;}" +
               "table, th, td {" +
               "border: 1px solid black;" +
               "border-collapse: seperate;}" +
               "</style>" +
               "</head>" +
               "<body>" +
               $"<img src={LogoUri} Width=300px Height=auto>" +
               "<h3>All Floors Report</br>" +
               $"{startDate.ToString("dd/MM/yyyy")} - {endDate.ToString("dd/MM/yyyy")}</h3>" +
               "<div>" +
               "<table>" +
               "<tr>" +
               "<th> Floor #</th>" +
               "<th> Rooms # </th>" +
               "<th> Reservations </th>" +
               "<th> Total Revenue </th></tr>";
            //Loop through all rooms
            string table = string.Empty;
            List<string> floors = new List<string>();
            List<decimal> floorTotals = new List<decimal>();
            List<int> floorReservations = new List<int>();
            foreach (Floor floor in floorList)
            {
                //Calculate revenue per floor
                int reservationCount = 0;
                decimal total = 0;
                foreach (Room room in floor.RoomList)
                {
                    foreach (RoomReservation reservation in room.RoomReservationList)
                    {
                        reservationCount++;
                        foreach (Transaction transaction in reservation.TransactionList)
                        {
                            total += transaction.ToPayAmount;
                        }
                    }
                }
                //Add floor to table
                table +=
                    "<tr>" +
                    "<td>" + floor.ElementNumber + "</td>" +
                    "<td>" + floor.RoomList.Count + "</td>" +
                    "<td>" + reservationCount + "</td>" +
                    "<td>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td>" +
                    "</tr>";
                //Add info for piechart
                floors.Add($"Floor {floor.ElementNumber}");
                floorReservations.Add(reservationCount);
                floorTotals.Add(total);
            }
            template += table;
            string templateEnd =
                "</table>" +
                PieChart(floors, floorReservations, "Floors by Reservation", "Floor", "Reservations", 1, false) +
                PieChart(floors, floorTotals, "Floors by revenue", "Floor", "Total", 2, false);
            template += templateEnd;
            HTMLStringToPdf(template, $"AllFloorsReport_{DateTime.Now.Year}{DateTime.Now.Month.ToString().PadLeft(2, '0')}{DateTime.Now.Day.ToString().PadLeft(2, '0')}", user);
        }

        public static void ReservationsByCustomer(Customer customer, User user, DateTime startDate, DateTime endDate)
        {
            string template =
                "<!DOCTYPE html>" +
                "<html>" +
                "<head>" +
                "<title>Reservations By Customer </title>" +
                "<link rel =\"stylesheet\" type = \"text/css\" href =\"style.css\"/>" +
                "<style>" +
                "table, th, td {" +
                "border: 1px solid black;" +
                "border-collapse: seperate;}" +
                "</style>" +
                "</head>" +
                "<body>" +
                $"<img src={LogoUri} Width=300px Height=auto>" +
                "<h3>Customer: " + customer.FName + " " + customer.LName + "</br>" +
                $"{startDate.ToString("dd/MM/yyyy")} - {endDate.ToString("dd/MM/yyyy")}</h3>" +
                "<h4>Phone Number: " + customer.PhoneNumber + "</h4>" +
                "<div>" +
                "<table>";
            //Loop through all reservations
            decimal allTotal = 0;
            foreach (var reservation in customer.RoomReservationList)
            {
                decimal total = 0;
                string partsTable = "<table>";
                //Loop through transactions in reservation
                foreach (var transaction in reservation.TransactionList)
                {
                    total += transaction.ToPayAmount;
                    partsTable +=
                        "<tr>" +
                        "<th> Service </th>" +
                        "<th> Price </th>" +
                        "</tr>";
                    //Loop through all transaction parts in transaction
                    foreach (var transactionpart in transaction.TransactionPartList)
                    {
                        partsTable +=
                            "<tr>" +
                            "<th>" + transactionpart.Service.Name + "</th>" +
                            "<th>" + transactionpart.Price + "</th>" +
                            "</tr>";
                    }
                    partsTable += "</table></br>";
                }

                allTotal += total;
                template +=
                    "<table>" +
                    "<tr>" +
                    "<th> Reservation #</th>" +
                    "<th> People Count</th>" +
                    "<th> Room </th>" +
                    "<th> Floor </th>" +
                    "<th> Check - In - Time </th>" +
                    "<th> Check - Out - Time </th>" +
                    "<th>Order Total</th>" +
                    "</tr>";
                //Add fields to table
                template +=
                    "<tr>" +
                    "<td>" + reservation.RoomReservationId.ToString() + "</td>" +
                    "<td>" + reservation.PeopleCount + "</td>" +
                    "<td>" + reservation.Room.ElementNumber.ToString() + "</td>" +
                    "<td>" + reservation.Room.Floor.ElementNumber.ToString() + "</td>" +
                    "<td>" + reservation.StartTime.ToString() + "</td>" +
                    "<td>" + reservation.EndTime.ToString() + "</td>" +
                    "<td>" + total.ToString("C", CultureInfo.CurrentCulture) + "</td>" +
                    "</tr>" +
                    "</table></br>";
                template += partsTable;
            }
            string templateEnd =
                "</table>" +
                "<h3> Total=" + allTotal.ToString("C", CultureInfo.CurrentCulture) + "</h3>";
            template += templateEnd;
            Debug.WriteLine(template);
            HTMLStringToPdf(template, $"{customer.FName}_{customer.LName}_Report{DateTime.Now.Year}{DateTime.Now.Month.ToString().PadLeft(2, '0')}{DateTime.Now.Day.ToString().PadLeft(2, '0')}", user);
        }

        /**
       PDF from HTML String
       anchor-html-string-to-pdf-with-ironpdf
          **/
        private static void HTMLStringToPdf(string htmlstring, string filename, User user)
        {
            // Add closing syntax and render HTML string to PDF
            htmlstring +=
                "</div>" +
                "<div style=\"padding-top : 50%\"><h4>Created By: "
                + user.FName + " "
                + user.LName + "</br>" +
                $"{DateTime.Now}</h4></div>" +
                "</body>" +
            "</html> ";
            Console.WriteLine(htmlstring);
            var Renderer = new IronPdf.ChromePdfRenderer();
            using (var PDF = Renderer.RenderHtmlAsPdf(htmlstring))
            {
                Renderer.RenderingOptions.TextFooter = new HtmlHeaderFooter() { HtmlFragment = "<div style='text-align:right'><em style='color:pink'>page {page} of {total-pages}</em></div>" };
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
                saveFileDialog.FileName = filename;
                saveFileDialog.DefaultExt = ".pdf";
                saveFileDialog.Filter = "PDF documents (.pdf)|*.pdf";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    PDF.SaveAs(saveFileDialog.FileName);
                    Renderer.RenderingOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen;
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Pie Chart Generator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="categories"></param>
        /// <param name="values"></param>
        /// <param name="title"></param>
        /// <param name="catName"></param>
        /// <param name="valueType"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string PieChart<T>(List<string> categories, List<T> values, string title, string catName, string valueType, int num, bool count)
        {
            string chart = string.Empty;
            chart += $"<div id = \"piechart_{num}\"></div>" +
                    "<script type = \"text/javascript\" src = \"https://www.gstatic.com/charts/loader.js\" ></script >" +
                    "<script type = \"text/javascript\">" +
                    // Load google charts
                    "google.charts.load('current', { 'packages':['corechart']});" +
                    $"google.charts.setOnLoadCallback(drawChart_{num});" +
                    // Draw the chart and set the chart values
                    $"function drawChart_{num}()" +
                    "{" +
                    "var data = google.visualization.arrayToDataTable([" +
                    $"['{catName}', '{valueType}'],";
            //Iterate through lists and add to string
            for (int i = 0; i < categories.Count; i++)
            {
                chart += $"['{categories[i]}',{values[i]}]";
                if (i != categories.Count - 1)
                    chart += ',';
            }
            // Optional; add a title and set the width and height of the chart
            chart += "]);" +
                "var options = { 'title':'" + title + "'," + " 'width':550, 'height':400";
            if (count)
                chart += ",'pieSliceText':value";
            chart += "};" +
            // Display the chart inside the <div> element with id="piechart"
            $"var chart = new google.visualization.PieChart(document.getElementById('piechart_{num}'));" +
            "chart.draw(data, options);" +
            "}" +
            "</script><br/><br/><br/>";
            return chart;
        }
        private static string BarChart<T>(List<string> categories, List<T> values, string title, string catName, string valueType, int num, bool count)
        {
            string chart = string.Empty;

            chart += "<div>" +
                "<script type =\"text/javascript\" src =\"https://www.gstatic.com/charts/loader.js\"></script >" +
                "<div id=\"myChart\" style=\"width:100%; max-width:600px;height:500px;\"></div>" +
                "<script>" +
                "google.charts.load('current',{packages:['corechart']});" +
                "google.charts.setOnLoadCallback(drawChart);" +
                "function drawChart() {" +
                // Set Data
                "var data = google.visualization.arrayToDataTable([" +
                $"['{catName}','{valueType}'],";
            for (int i = 0; i < categories.Count; i++)
            {
                chart += $"['{categories[i]}',{values[i]}]";
                if (i != categories.Count - 1)
                    chart += ',';
            }
            chart += "]);";
            // Set Options
            chart += "var options = {" +
            $"title: '{title}'," +
            "hAxis: { title: '" + catName + "'}," +
            "vAxis: { title: '" + valueType + "'}," +
            "legend: 'bottom'" +
            "};" +
            // Draw
            "var chart = new google.visualization.LineChart(document.getElementById('myChart'));" +
            "chart.draw(data, options);" +
            "}" +
            "</script>";
            chart += "</div><br/><br/><br/>";
            return chart;
        }
    }
}
