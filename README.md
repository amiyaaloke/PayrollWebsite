

# dotnetcore - Payroll Website

# Ubuntu 16.04

SQL Server Installation

1. Import the public repository GPG keys:
wget -qO- https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -

2. Register the Microsoft SQL Server Ubuntu repository:
sudo add-apt-repository "$(wget -qO- https://packages.microsoft.com/config/ubuntu/16.04/mssql-server-2017.list)"

3. Run the following commands to install SQL Server:
sudo apt-get update
sudo apt-get install -y mssql-server

4. After the package installation finishes, run mssql-conf setup and follow the prompts to set the SA password and choose your edition.
sudo /opt/mssql/bin/mssql-conf setup

5. Once the configuration is done, verify that the service is running:
systemctl status mssql-server


