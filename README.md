I dette prosjektet har vi laget en Quiz app med en database for å lagring av quizer. 
Vi har brukt ASP.NET framework for å lage en applikasjon med MVC (model-view-controller) arkitektur. 
I mappestrukturen har vi:
Views: Dette er CSHTML/razor sider som viser data til brukeren. Views viser viewmodel data, og sender data tilbake til controllerne.
Models: Her har vi data og logikk for hvordan data skal struktureres. 
Controllers: Dette er koblingen mellom views og databasen. Controllerne snakker med DAL/Models, og sender data til View.
Viewmodels: Dette er tilpassede datapakker til visning. Brukes for å vise akkurat det viewet trenger og ikke alt fra modellen.
Data access layer: Her håndteres kommunikasjonen med databasen. CRUD håndteres her. Vi bruker entity framework for å kunne kommunisere med databasen ved å bruke dotnet objekter/ c# klasser

Filtrering av quizer er kun et prof of consept.

For å kjøre prosjektet må man: 
cd Que
dotnet build
dotnet run

Gruppemedlemmer: Erik Grimstveit s385500, Da Quynh Truong s385550, Zoey Retzius s380936, Sara Solstad Wessel-Hansen s385572, Arthur Thonrud Flotvik s369519
