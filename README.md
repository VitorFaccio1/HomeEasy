# **HomeEasy v1**    

----
 
* [Guidelines](#**guidelines**)
* [Controllers](#**controllers**)
    * [User](#user)
        * [Add User Async v1.](#add-user-async-v1.)
        * [Get Users Async v1.](#get-users-async-v1.)

## **Controllers**

### User

#### - Add User Async v1.

- [ *POST* /api/homeeasy/v1/user ]

	- **Description:** This method add a user to the system.

	- **Request:**
		```json
		{
	        "name": "string",
            "email": "string",
            "password": "string"
		}
		```
	- **Response:**
		* *200 - OK*           
		```json 
		{}
		```

#### - Get Users Async v1.

- [ *GET* /api/homeeasy/v1/users ]

	- **Description:** This method gets all the users registered in the system.

	- **Request:**
		```json
		{}
		```
	- **Response:**
		* *200 - OK*           
		```json 
		{
	        "name": "string",
            "email": "string",
            "password": "string"
		}
		```
