# Hangfire.MissionControl

![dashboard](content/dashboard.png)

A plugin for Hangfire that enables you to launch jobs manually.

Read about hangfire here: https://github.com/HangfireIO/Hangfire#hangfire-
and here: http://hangfire.io/

## Instructions
NuGet package will be deployed later.

Setup your dashboard code:
```csharp
services.AddHangfire(configuration => configuration.UseMissionControl());
```

## License
Authored by: Viktor Svyatokha (ahydrax)

This project is under MIT license. You can obtain the license copy [here](https://github.com/ahydrax/Hangfire.MissionControl/blob/master/LICENSE).