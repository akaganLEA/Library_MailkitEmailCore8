This is a legacy C#.NET library for sending emails through hard-coded relay servers. 
For high-volume email processes we need to route email through SMTP2GO relays. Our corporate exchange service on outlook.com will lock the sender account if it exceeds 30 emails per minute.
For encrypted emails we need to route through our barracuda encryption service, which can only be reached through our outlook.com corporate email relay. 
If we are required to send large volumes of encrypted email, the process calling the library MUST throttle the calls to < 30 minute. Typically a 3-second (3000 ms) WAIT statement is inserted in the loop sending emails to prevent account lockouts on outlook.com
The library has hard-coded authentication that must be updated and recompiled if any changes are made to either mail relay configuration.
The library is periodically re-compiled with updated versions of Mailkit and published with the version number so the proper Mailkit library can be referenced in the calling app.
Versions of the library are maintained at: \\vfs01\programming\Libraries 
Previous project versions of the library are in the Subversion CVS: https://svn.leadingedgeadmin.com:9993/svn/Lib_MailkitEmail_Core8/
