A significant expansion of the current karma and award system. This will track every reaction including karma and awards.

## Tables
### KarmaEmotes
How much karma different emotes give.

| Emote (pkey) | Amount (int) |
| ------------ | ------------ |
### Reactions
Stores every reaction for every emote.

| Id  | Giver (refs Users.Id) | Receiver (refs Users.Id) | MessageId | Emote | Timestamp (UTC) |
| --- | --------------------- | ------------------------ | --------- | ----- | --------------- |

## Operations
- Add reaction
- Reaction removed from Discord → remove row
- User deleted → remove their reactions
- Message deleted → remove reactions targeting it

### Reactions
- Get all reactions received from a user for the year (or any year)
- Get all reactions received from a user for all time
- Get all reactions received from all users for the year (or any year)
- Get all reactions received from all users for all time

- Get all reactions sent to a user for the year (or any year)
- Get all reactions sent to a user for all time
- Get all reactions sent to all users for the year (or any year)
- Get all reactions sent to all users for all time

### Karma
- Get all karma from a user for the year (or any year)
- Get all karma from a user for all time
- Get all karma from all users for the year (or any year)
- Get all karma from all users for all time

- Get all karma sent to a user for the year (or any year)
- Get all karma sent to a user for all time
- Get all karma sent to all users for the year (or any year)
- Get all karma sent to all users for all time

#### KarmaEmotes
- Add emote to KarmaEmotes
- Remove emote from KarmaEmotes
- Modify amount
