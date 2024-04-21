# Hotel Management Program

## Final Project at TCB

#### Disclaimer

The code and visuals in this repository are for viewing purposes only. They are not intended for commercial use or redistribution without permission.

#### Rebuilt project to allow the XAML graphic editor to work.

#### Login data:
* Username: arij | Password: pass1 | Role: Admin
* Username: ayelet | Password: pass2 | Role: Manager
* Username: worker | Password: pass3 | Role: Worker

#### Information:
This is a hotel reservation software developed in .NET WPF with SQL Database integration. The database in the repository is stored locally as a MSAccess file, but it can be quickly changed to use an SQL server. The application follows the MVVM architecture.
Reservations are customer-oriented and the room selection is done through a visual representation of the hotel floors vertically and horizontally (Floors and rooms in each floor).

#### Images

**Login** (To the right is a news RSS reader blurred in the images since they might be triggering)</br></br>
![Login](Images/Login.jpg)

**Reservation Process**
- **Customers Screen:** Choose customer. The empty row at the bottom is used to add new customers. Data validation is customizable and revertible if a field results in an error.
![Customers](Images/Customers.jpg)

- **Floor View:** Choose a floor.
![Floor](Images/FloorView.jpg)

- **Reservation:** Choose an available room on the floor. Rooms will be marked with a red border if they are not available at the requested time, if no customer is chosen, or if their capacity is lower than needed. Rooms with a green border are valid for the time and capacity and can be reserved.</br>
Cost is calculated based on easily customizable parameters in code for policy, and service menu to adjust prices. Weekdays and Weekends can have different rates and calculated based on dates so an order could be a combination of different costs for different days.</br>
![Reservation1](Images/Reservation1.jpg)

- **Reservation:** Credit card dummy interface.
![Reservation2](Images/Reservation2.jpg)

- **Reservation:** Room reserved.
![Reservation3](Images/Reservation3.jpg)

- **Reservation:** Output reservation confirmation PDF.
![Reservation4](Images/ReservationForm.jpg)

**Reports**
- **Available Reports:** Customer-specific reports require choosing them in the Customers screen.
![Reports1](Images/Reports.jpg)

- **Reservation Report:** Example for a date range.
![Reports2](Images/ReservationReport.jpg)

#### Challenges
* Developing the software in MVVM architecture without using code-behind required learning some more advanced binding techniques, especially when dealing with DataTables and custom UI elements.
* Creating a single-window application with changing panels while trying to decouple the screens from each other posed challenges, especially when some screens needed information from others.

#### What I Would Have Done Differently
* More use of design patterns to solve some issues more elegantly.
* Designing the SQL <-> C# process from scratch using composition of SQL abstraction modules instead of inheritance chains, which made the code messy.
* More composition over inheritance in general.
* Better separation of concerns, especially in the core ViewModels of the screens.
* Cleaner SQL reading with an abstraction for more fine-grained control over reading and making SQL reading a non-blocking async operation for scalability.
* Modularizing parts with similar logic for reusability.
