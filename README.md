# Azure Cosmos DB: Table API
This is the repository for the LinkedIn Learning course Azure Cosmos DB: Table API. The full course is available from [LinkedIn Learning][lil-course-url].

![Azure Cosmos DB: Table API][lil-thumbnail-url] 

Azure supports many flavors of NoSQL databases, including the popular key-value table approach. This is the simplest type of NoSQL database—each entry is identified by a unique key, yet the data format is flexible, and the data store is extremely scalable.
There are two ways to provision NoSQL table-based data in Azure: Azure Table Storage and Cosmos DB Table API. There are advantages to both, and in this course instructor Walt Ritscher describes the differences between the two service models, so you can choose the best model for your data. Most of this course looks at the Cosmos DB option, and Walt shows you how to provision a Cosmos DB database and edit data in the tables, how to access the data with the REST API and the .NET SDKs, and how to query and manipulate the data. Finally, he looks at how to scale out the database to other Azure regions.

## Instructions
This repository has branches for each of the videos in the course. You can use the branch pop up menu in github to switch to a specific branch and take a look at the course at that stage, or you can add `/tree/BRANCH_NAME` to the URL to go to the branch you want to access.

## Branches
The branches are structured to correspond to the videos in the course. The naming convention is `CHAPTER#_MOVIE#`. As an example, the branch named `02_03` corresponds to the second chapter and the third video in that chapter. 
Some branches will have a beginning and an end state. These are marked with the letters `b` for "beginning" and `e` for "end". The `b` branch contains the code as it is at the beginning of the movie. The `e` branch contains the code as it is at the end of the movie. The `main` branch holds the final state of the code when in the course.

When switching from one exercise files branch to the next after making changes to the files, you may get a message like this:

    error: Your local changes to the following files would be overwritten by checkout:        [files]
    Please commit your changes or stash them before you switch branches.
    Aborting

To resolve this issue:
	
    Add changes to git using this command: git add .
	Commit changes using this command: git commit -m "some message"


### Instructor

Walt Ritscher 
                            
Senior Staff Author

                            

Check out my other courses on [LinkedIn Learning](https://www.linkedin.com/learning/instructors/walt-ritscher).

[lil-course-url]: https://www.linkedin.com/learning/azure-cosmos-db-table-api
[lil-thumbnail-url]: https://cdn.lynda.com/course/2448119/2448119-1647538859664-16x9.jpg
