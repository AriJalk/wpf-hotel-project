using HotelProject.Model.DbClasses;
using IronPdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject.ViewModel.Helpers.Pdf
{
    /// <summary>
    /// Class for generating reports
    /// First an HTML code is generated from template
    /// than it is passed to IronPDF for conversion to PDF 
    /// </summary>
    public static class HtmlReports
    {
        /// <summary>
        /// Generates A Reservation Document for the client
        /// </summary>
        /// <param name="reservation"></param>
        public static void ReservationTicket(RoomReservation reservation, User user)
        {
            string template =
                 "<!DOCTYPE html>" +
                 "<html>" +
                 "<head>" +
                 " <title>[Title] </title>" +
                 " <link rel =\"stylesheet\" type = \"text/css\" href =\"style.css\"/>" +
                 "</head>" +
                 "<body>" +
                 "<h2>Hotel Name</h2>" +
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
                 "</h4>";
            HTMLStringToPdf(template,$"Reservation{reservation.RoomReservationId}",user);
        }

        /// <summary>
        /// Generates a summary of all reservations
        /// </summary>
        /// <param name="reservationList"></param>
        public static void AllReservations(List<RoomReservation> reservationList, User user)
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
                "<h2>Hotel Name</h2>" +
                "<h3>All Reservations Report</h3>" +
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
                    "<td>" + total + "</td>" +
                    "</tr>";
            }
            template += table;
            string templateEnd =
                "</table>" +
                "<h3> Total=" + allTotal.ToString() + "</h2>";
            template += templateEnd;
            HTMLStringToPdf(template,$"AllReservationsReport_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}", user);
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
                "<h2>Hotel Name</h2>" +
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
                    "<td>" + total + "</td>" +
                    "</tr>";
            }
            template += table;
            string templateEnd =
                "</table>";
            template += templateEnd;
            HTMLStringToPdf(template, $"AllCustomersReport_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}", user);
        }

        public static void AllRoomsReport(List<Room> roomList, User user)
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
               "<h2>Hotel Name</h2>" +
               "<h3>All Rooms Report</h3>" +
               "<div>" +
               "<table>" +
               "<tr>" +
               "<th> Room #</th>" +
               "<th> Floor # </th>" +
               "<th> Reservations </th>" +
               "<th> Total Revenue </th></tr>";
            //Loop through all rooms
            string table = string.Empty;
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
                    "<td>" + room.RoomReservationList.Count + "</td>" +
                    "<td>" + total + "</td>" +
                    "</tr>";
            }
            //Add fields to table



            template += table;
            string templateEnd =
                "</table>";
            template += templateEnd;
            HTMLStringToPdf(template, $"AllRoomsReport_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}",user);
        }

        public static void AllFloorsReport(List<Floor> floorList, User user)
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
               "<h2>Hotel Name</h2>" +
               "<h3>All Floors Report</h3>" +
               "<div>" +
               "<table>" +
               "<tr>" +
               "<th> Floor #</th>" +
               "<th> Rooms # </th>" +
               "<th> Reservations </th>" +
               "<th> Total Revenue </th></tr>";
            //Loop through all rooms
            string table = string.Empty;
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
                    "<td>" + total + "</td>" +
                    "</tr>";
            }
            template += table;
            string templateEnd =
                "</table>";
            template += templateEnd;
            HTMLStringToPdf(template, $"AllFloorsReport_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}",user);
        }

        public static void ReservationsByCustomer(Customer customer, User user)
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
                "<h2>Hotel Name</h2>" +
                "<h3>Customer: " + customer.FName + " " + customer.LName + "</h3>" +
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
                        "<tr>"+
                        "<th> Service </th>"+
                        "<th> Price </th>" +
                        "</tr>";
                    //Loop through all transaction parts in transaction
                    foreach (var transactionpart in transaction.TransactionPartList)
                    {
                        partsTable +=
                            "<tr>" +
                            "<th>" + transactionpart.Service.Name + "</th>" +
                            "<th>" + transactionpart.Price + "</th>"+
                            "</tr>";
                    }
                    partsTable += "</table></br>";
                }
                    
                allTotal += total;
                template +=
                    "<table>"+
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
                    "<td>" + total + "</td>" +
                    "</tr>" +
                    "</table></br>";
                template += partsTable;
            }
            string templateEnd =
                "</table>" +
                "<h3> Total=" + allTotal.ToString() + "</h2>";
            template += templateEnd;
            Debug.WriteLine(template);
            HTMLStringToPdf(template,$"{customer.FName}_{customer.LName}_Report{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}",user);
        }

        /**
       PDF from HTML String
       anchor-html-string-to-pdf-with-ironpdf
          **/
        private static void HTMLStringToPdf(string htmlstring, string filename, User user)
        {
            // Render any HTML fragment or document to HTML
            htmlstring+=
                "</div>"+
                "<h4>Created By: "
                + user.FName + " " 
                + user.LName + "</h4>" +
            "</body>" +
            "</html> ";
            var Renderer = new IronPdf.ChromePdfRenderer();
            using (var PDF = Renderer.RenderHtmlAsPdf(htmlstring))
            {
                Renderer.RenderingOptions.TextFooter = new HtmlHeaderFooter() { HtmlFragment = "<div style='text-align:right'><em style='color:pink'>page {page} of {total-pages}</em></div>" };
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
                saveFileDialog.FileName = filename;
                saveFileDialog.DefaultExt = ".pdf";
                saveFileDialog.Filter= "PDF documents (.pdf)|*.pdf";
                saveFileDialog.ShowDialog();
                PDF.SaveAs(saveFileDialog.FileName);
                Renderer.RenderingOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen;
            }
        }
    }
}
