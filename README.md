# CsvToMongoTool
 
**IMPORTANT**
ADD THIS CLASS TO THE PROGRAM!:

static class MongoConnection
    {
        public static MongoClient dbClient = new MongoClient(//LOCALHOST);
        public static IMongoDatabase usersDb = dbClient.GetDatabase(//YOUR DATABASE NAME!);
        public static IMongoCollection<Employee> dbCollector = usersDb.GetCollection<Employee>(YOUR COLLECTION NAME);
    }

CSV TOOL!
run from the command line dotnet run and pass some args
*give some args to the command line

*ARGS MUST CONTAIN CSV FILES/LINKS!

*DECOMPRESSED ONLY GZIP FILES! (.gz type).

*THE PROGRAM WILL TAKE EMPLOYEES, WHICH MUST HAVE: NAME, LAST NAME, AGE, JOINED DATE.

DONT FORGET TO CONNECT TO THE MongoDB!!

ENJOY! 