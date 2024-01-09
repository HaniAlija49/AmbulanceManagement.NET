# Ambulance Management System
## Overview

The Ambulance Management System is a web-based solution designed to streamline and enhance the efficiency of ambulance services and associated administrative tasks within healthcare settings. This repository contains the source code and documentation for the project.

## Features

- **Appointment Scheduling:** Efficient and automated scheduling of appointments for patients.
- **Patient Records Management:** Centralized storage and management of patient information.
- **Staff Management:** Maintenance of records for doctors, nurses, and administrative personnel.
- **Report Generation:** Creation and storage of reports related to patient appointments.

## Installation

To run the Ambulance Management System locally, follow these steps:

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/HaniAlija49/AmbulanceManagement.git
   ```

2. **Navigate to the Project Directory:**
   ```bash
   cd AmbulanceManagement
   ```

3. **Run the Application:**
   - Open the solution file `AmbulanceManagement.sln` in Visual Studio.
   - Build and run the project.

4. **Database Setup:**
   - The application uses SQL Server. Ensure you have a SQL Server instance available.
   - Update the connection string in the `appsettings.json` file with your SQL Server details.
   - Run the database migrations to create the required tables.

5. **Login with Default Account:**
   - Use the following credentials for the first login:
     - **Email:** admin@gmail.com
     - **Password:** Admin123@

## Technologies Used

- **ASP.NET Core:** Web framework for building modern, cloud-based, and internet-connected applications.
- **Entity Framework Core:** Object-relational mapping (ORM) framework for .NET.
- **SQL Server:** Database management system used for data storage.
- **HTML, CSS, JavaScript:** Front-end technologies for the user interface.

## Project Structure

- **`/src`:** Contains the source code for the Ambulance Management System.
- **`/docs`:** Documentation related to the project.

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, feel free to open an issue or submit a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- The Ambulance Management System was developed by [Hani Alija](https://github.com/HaniAlija49), [Jasin Ismaili](https://github.com/jasini1), and [Uvejs Murtezi](https://github.com/uvejsm) as part of their advanced programming project at Southeast European University.

Thank you for checking out the Ambulance Management System!
