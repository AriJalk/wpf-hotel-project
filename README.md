# Hotel Management program

## Final project at TCB

### Disclaimer

The code and visuals in this repository are for viewing purposes only. They are not intended for commercial use or redistribution without permission.

### Rebuild project to allow the xaml graphic editor to work

### Login data:
* arij - pass1 - Admin
* ayelet - pass2 - Manager
* worker - pass3 - Worker

### Information:
A hotel reservation software, done in .NET WPF with SQL Database integration (The DB in the repository is stored locally as a MSAccess file, but should be able to change SQL server relatively fast) with MVVM architecture.
Reservations are customer oriented and the room selection is done through a visual respresentation of the hotel floors vertically and horizontaly (Floors and rooms in each floor).

### Images

Login
![Login](Images/Login.jpg)</br>
### Reservation proccess
Customers Screen - Choose customer, the buttom empty row is used to add new custumers, data validation is customizable and revertable if a field results in an error.
![Customers](Images/Customers.jpg)</br>
Floor View - Choose floor
![Floor](Images/FloorView.jpg)</br>

Reservation - Choose available room in the floor, rooms in the floor will be marked with red border if they are not available either at the requested time or if no customer is chosen, or if their capacity is lower than needed, rooms with green border are valid for the time and capacity and can be reserved.
![Reservation1](Images/Reservation1.jpg)</br>
Reservation - Credit card dummy interface
![Reservation2](Images/Reservation2.jpg)</br>
Reservation - Room reserved
![Reservation3](Images/Reservation3.jpg)</br>
Reservation - Output reservation confirmation PDF 
![Reservation4](Images/ReservationForm.jpg)</br>

Reports - Available reports, customer specific reports requires choosing them in the customers screen.
![Reports1](Images/Reports.jpg)</br>
Reports - Reservation report example for date range.
![Reports1](Images/ReservationReport.jpg)</br>
