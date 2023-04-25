# Trial Day task

This solutions contains 4 projects:
- EventContracts (should not be changed) - Shared Library which contains *INTERNAL* Event Contracts. This library can be referenced in IntegrationLayer or other projects created by you. 
- EventProducer (should not be changed) - This is part of the systems which non-stop generates events, treat it as a live MaaS platform backend, where user's interactions ends up in some event being produced.  
- IntegrationLayer - this is the missing piece of the puzzle. 

### Task
Business goal - integration with `CRM`. This CRM tool is used by the client to do:
- customer support
- run marketing campaigns
- issue refunds
- understand trip usage patterns
- etc

At the time being client does not have enough capacity to integrate to our public event stream, there is also an issue that public event stream currently does not exist. Your task is to integrate with `CRM` and also build _public_ event stream.

We have already agreed with client that we will be using Kafka as our event streaming platform.

Client provided only `CRM Api` description we need to integrate with. No live access to CRM, reasons are not clear for us, but time is money, so we need to implement very thin layer of said api so we could integrate with it instead. We can think of it like of a `mock CRM`. This mock should be rest api service we can run and it would store data in memory.

### CRM API Description

This is the yaml file we received from the client
```
api:
  users:
    get:
      description: Returns all registered users
      returns:
        ok: contracts.User[]
    get {id}:
      description: Returns registered user by id
      returns:
        ok: contracts.User
        notfound:
    post:
      description: Registers new user
      returns:
        created: contracts.User
    post {id}:
      description: Replaces existing user properties by user id
      returns:
        ok: contracts.User
        notfound:
      
  trips:
    get:
      description: Returns all trips
      returns:
        ok: contracts.Trip[]
    get {id}:
      description: Returns trip by id
      returns:
        ok: contracts.Trip
        notfound:
    post:
      description: Creates new trip
      returns:
        created: contracts.Trip
    post {id}:
      description: Replaces existing trip properties by trip id
      returns:
        ok: contracts.Trip
        notfound:

contracts:
  User:
    Id: string
    LastName: string
    FirstName: string, nullable
    Email: string
    LastTripType: string, nullable
    LastTripId: string, nullable
    LastTripDate: datetime
  Trip:
    Id: string
    UserId: string
    From: string, nullable
    To: string, nullable
    Price: number
    StartDate: datetime
    EndDate: datetime, nullable
    Type: string
```

### Bonus points
- We have not worked with .Net's minimal API at Trafi, this is a great opportunity to write this mock using that
- Mocked service has GUI API explorer
- GDPR is of highest priority for Trafi. How can we ensure user's data can be deleted from Kafka

### Env setup
Start event streaming backbone (Kafka)
```
docker-compose up -d
```

Build `EventProducer` . Start both console app in the background. It will start producing events.

That is it. You have a running platform!

### Code commit instructions
Create new branch `{name}_{date}` (like `John_2023-01-01`) and push all changes to the branch.