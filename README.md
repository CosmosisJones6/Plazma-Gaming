# ![logosmall](https://user-images.githubusercontent.com/69151140/215780193-e8540d17-d6eb-4b44-834c-82acec19dec4.png) Plazma Gaming  


## About the project

Plazma Gaming is an online video game selling service focusing on video games created by individual or small group of developers. We give new game ideas the opportunity to grow and to become something bigger by providing a platform for them. 

This solution hadles both the backend and the frontend with more focus and detail on the backend. This semester project for university was created in Visual Studio .NET 6, using C#, HTML, JavaScript, CSS.

## Architecture

The architecture consists of multiple tiers: 
- (Relational database (not in repository))
- ASP .NET CORE REST API
- ASP .NET CORE MVC
- Windows Forms


![image](https://user-images.githubusercontent.com/69151140/215792153-4857dd6d-7102-4aec-9fa9-3f0f2aaa6491.png)


## ASP .NET CORE MVC : Website

On our website buying games, registering for events and showing additional information like news and developer info are available and also developers can reach out to us through the contact form if they would like to have their games listed for sale on the website.

MVC stands for Model-View Controller, which is a software architectural pattern. It uses models to create views that are then accessed through the controllers. Communicating with the API is implemented using the [RestSharp](https://restsharp.dev/) framework.


![website](https://user-images.githubusercontent.com/69151140/215776160-317adc53-1a73-4d65-853f-1d9149ff6fcb.gif)


## Windows Forms : Desktop App

The desktop application is for modifying information in the database. This application is only used by our staff members, it isn't accessable by the public.
We have a login form so only authorized people can access the main form.

Communicating with the API is implemented using the [RestSharp](https://restsharp.dev/) framework.


![login](https://user-images.githubusercontent.com/69151140/215770589-f6c10019-7d6e-4875-bbc1-a6d6744d280b.png)


![mainform](https://user-images.githubusercontent.com/69151140/215776181-aa5034ce-9300-46a6-bc83-4e5921be7528.gif)


## ASP .NET CORE REST API : API

The API handles data transfer between the database and system endpoints. 
The API is a RESTful web API, that uses HTTP methods to access resources via URL-encoded parameters and uses JSON to transmit data.

Also, the API handles a concurrency problem with a transaction, which occures when multiple people want to register for the last place in an event.

The api is secured with authentication, which means, one can only access information through the api if they have the token with the corresponding role sent with the request.

When running the API a Swagger interface is available to help with testing.

Login passwords are salted and hashed with the [BCrypt](https://www.nuget.org/packages/BCrypt.Net-Next#readme-body-tab) framework for security purposes.


![swagger](https://user-images.githubusercontent.com/69151140/215794232-d922b94d-3014-46e2-911a-cba27c0af2bb.png)


