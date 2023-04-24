# Trial Day task

This solutions contains 4 projects:
- EventContracts (should not be changed) - Shared Library which contains *INTERNAL* Event Contracts. This library can be referenced in IntegrationLayer or other projects created by you. 
- EventProducer (should not be changed) - This is part of the systems which non-stop generates events, treat it as a live MaaS platform backend, where user's interactions ends up in some event being produced.  
- InMemoryCRM (should not be changed) - This is a 3rd party system we need to integrate with. (hint: it has swagger)  
- IntegrationLayer - this is the missing piece of the puzzle. 

### Task
Business goal - integration with `CRM` aka `InMemoryCRM`. This CRM tool is used by the client to do:
- customer support
- run marketing campaigns
- issue refunds
- understand trip usage patterns
- etc

At the time being client does not have enough capacity to integrate to our public event stream, there is also an issue that public event stream currently does not exist. Your task is to integrate with `CRM` and also build _public_ event stream.

We have already agreed with client that we will be using Kafka as our event streaming platform.

### Bonus point
GDPR is of highest priority for Trafi. How can we ensure user's data can be deleted from Kafka.

### Env setup
Start event streaming backbone (Kafka)
```
docker-compose up -d
```

Build `EventProducer` and `InMemoryCRM`. Start both console apps in the background.

Try opening `InMemoryCRM` swagger page on your browser.

That is it. You have a running platform!

### Code commit instructions
Create new branch `{name}_{date}` (like `John_2023-01-01`) and push all changes to the branch.