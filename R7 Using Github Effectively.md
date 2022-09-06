# **R7 Using Github Effectively <sup>1</sup>**

    
## Purpose

Imagine being in a software development team where you have 5 to 10 other teammates. You have an idea for working on something that could make your software better to use and you spend 20 hours working on it by yourself. When you finally push it to your team’s repository, your supervisor does not approve many parts of your implementation due to many reasons, including inefficiencies that have been tested before and proprietary issues, among others. Not only have you spent 20+ hours working on a feature that will not be integrated, you also have to do the work again. There must be a way to get feedback about what you are doing while you are working so that you do not waste, right?

**Well, there is!**

Software development teams use **Issues** feature on GitHub to avoid wasting time and to document anything they plan on working on. This accounting system can include tasks such as finding bugs, ideas, testing ideas and other specific problems with the project. The team members can pick which issue they want to address and so everyone knows who is working on what. The team member also knows exactly what they are doing and to focus on completing that single issue before moving on to another one. As issues are resolved, the team can quickly see progress and thus can cross items off the "to do list" and get the [dopamine burst](http://blog.idonethis.com/the-science-of-motivation-your-brain-on-dopamine/) and give a sense of accomplishment. It is also easy to leave specific comments on the progress of each issue so that whoever is currently working on them can get targeted feedback.

Sounds practically miraculous, right? Well, we need to first put the infrastructure in place to allow us to create, comment and resolve issues.

## **How to create issues on Github?**

1. On GitHub, navigate to the main page of your project repository.

2. Under your repository name, click **Issues**: 
![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Issues-Clicked.png "Select Issue")

3. Then select **New Issue**.
![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Issues-Button.png "Issues-Button")

4. Type a relevant title and a short description of what you are planning to fix or implement. 

5. Once you’ve added a title and description, click on the setting icon next to "Assignees" and select **your username**, **mario-nakazawa** and **belainehi**, essentially assigning yourself and your stakeholders to the issue. 
![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Assignees.png "Assignees")

6. Choose an appropriate **Label** by selecting the setting icon next to labels for your issue. 
![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Labels.png "Labels")

7. Finally click **Submit New Issue**. 
![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Submit%20new%20issue.png "Submit New Issue")

**That is it!**

## **Time to Work on the Issue on a different Branch.**

Once you’ve created an issue, you will start working on implementing your idea. It is good coding practice to work on your changes on a separate **branch** that is dedicated to solve this <ins>single</ins> issue. You do not want to keep working on your code on the main branch because it is difficult to debug, particularly if you have tried to work on multiple issues at once and the changes start to interfere with each other. If you need a refresher on how to create branches, read the documentation [here](https://docs.github.com/en/desktop/contributing-and-collaborating-using-github-desktop/making-changes-in-a-branch/managing-branches). Make the changes in that branch, knowing that your original code that worked before is "safe" and not break anything.

## **Time to Merge the Branches!**

After you worked on your code, **tested it** (this one is important), and you are confident that you are ready to combine it to your main branch, you need to create a **Pull Request**.

Pull Requests are used to indicate to your engineering team that you have code you’d like to integrate into the team’s project. In other words, you are ready for someone to review your code, which results in an approval, recommended changes, or a denial. Approval signifies that your peers reviewed your changes and your supervisor indicates that it is ready to be integrated into the product you are shipping. Alternatively, you may get specific comments on portions of your code that you must address first before your changes can be adopted. Denials may mean that your changes are either unacceptable or may have already been solved (which should not happen if all members of your team used issues correctly because you "claimed" it).

### **How to create pull requests on Github?**

1. On GitHub, navigate to the main page of your project repository.

2. Under your repository name, click **Pull requests**: 

![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Pull%20Requests/Pull%20Request%20Clicked.png "Pull Request")

3. Then select **New pull request**.

![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Pull%20Requests/New%20Pull%20Request%20Button.png "New Pull Request")

4. Select the two branches you would like to merge, make sure the branch on the right is the one you are merging into the branch on the left. 

![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Pull%20Requests/Select%20Separate%20Branches.png "Separate Branches")

Sometimes your two branches are not automatically able to merge if there is a conflict meaning that some of your work might get overwritten if you merge the branches. 

![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Pull%20Requests/Can't%20Automatically%20Merge.png "Can't merge automatically")

You can still create the pull request and make sure all changes in main are found in the branch you would like to merge. You can keep making changes on your branches after you’ve created  a pull request, the pull request will automatically include your new commits. 

5. Click **Create pull request**.

![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Pull%20Requests/Create%20Pull%20Request%20Button.png "Create pull request")

6. Add a relevant title and description. Make a reference to issues by using "#" + **your issue number**.

7. Click on the setting icon next to Reviewers and add the users **mario-nakazawa** and **belainehi** as reviewers.

![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Pull%20Requests/Reviewers.png "Reviewers")

8. Finally select **Create pull request**.
  
![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Create%20Pull%20Request.png "Create pull request")

  
## **To Submit**
Now that you know how to create Issues and Pull Requests, here are what we expect you to do by the due date indicated on Moodle:

1. Refer to your requirements.md document, pick at least 5 things you’d like to implement in your project, and create an issue for each one. 
  
    **Important**: These issues CANNOT be about how you plan to implement these issues, such as "research how to use <language or function>". It must be the issue itself.
  
2. Assign yourself, **mario-nakazawa** and **belainehi** to your issues.
  
3. Create a branch for each issue.
  
4. Pick at least two, preferably three issues, and implement the changes related to the issue.
  
5. Create pull requests for the weekly action items discussed in class. 
  
6. After you create a pull request, link an issue to it by clicking on the settings icon next to Linked Issues. This step might not be neccessary if you've made a reference in your Pull request to the issue. 

 ![alt text](https://github.com/BC-senior-projects/git-repo-setup-belainehi/blob/main/Pull%20Requests/Linked%20Issue.png "Link Issue").
  
7. Wait until our approval to merge and close your pull request even if you have completed working on that portion of your project. 

8. Keep on creating other issues and continue to work on your project! **You need to constantly be working on them to complete your project.**



    <sub>1. Created by Immanuela Belaineh and Mario Nakazawa for CSC493 Fall 2021</sub>

