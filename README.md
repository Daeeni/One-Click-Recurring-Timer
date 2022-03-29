# One-Click-Recurring-Timer

Small .NET 6 WPF application for people who have a daily recurring event and sometimes forget it. 


The App only has one button and a field to enter a time. If the button is not pressed up to 10 min before the time a Windows notification (incl. Sound) will accure every 2 minutes until the button is pressed. Uses Caliburn Micro framework and MVVM design pattern. The App will save the Date and Time of the button click locally (JSON Format).
