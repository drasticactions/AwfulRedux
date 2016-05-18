![alt text](http://i.imgur.com/iIuWbuj.png "Awful Forums Reader")

# What is this?!?

Awful Forums Reader is a Windows 10 UAP/Windows 8.1/Windows Phone 8.1 App. It allows you to access the [Something Awful](https://forums.somethingawful.com) on a variety of different devices; Table, Phone, Desktop, Xbox, IoT and Hololens. It's really intended for tablets and phones, but it does work well on Desktops and could be extended further.

![alt text](http://i.imgur.com/o96E4UE.png "Awful Forums Reader")

# How do I build this?!?

1. Check out [Awful Forums Library](https://github.com/drasticactions/AwfulForumsLibrary) and place it one directory above this project. I'm open to changing this so it's linked instead to this git, but I'm trying to figure out the best way to handle the seperate repos. Awful Forums Library is the core parsing library for the reader app. It handles getting and sending the http requests, as well as handling cookies and auth.
2. Open up the AFR solution, select a processor to build (x86/x64 for Desktops and Tablets, ARM for Phones) and that should be it! Package restore should get the rest of the packages.

# What about the 8.1 project?
You can check it out in the [legacy branch](https://github.com/drasticactions/Awful-Forums-Reader/tree/legacy). Same as above; clone Awful Forums Library one directory above that directory, and it should just build. 

# What's the difference between this an Awful-Forums-Reader?!?

This is an updated version that uses Template 10 for a back end framework. I could have branched it, but I didn't :effort:.
