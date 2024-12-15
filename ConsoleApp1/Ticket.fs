module Ticket

open System
open System.IO
open System.Text.Json

type Ticket( day: string,party:string,seat:int*int, userName: string) =

    let id = Guid.NewGuid()  

    member this.Id with get() = id  
    member this.Seat with get() = seat
    member this.Day with get() = day
    member this.Party with get() = party
    member this.UserName with get() = userName

let createTicket day party seat userName = Ticket(day,party,seat,userName)


let appendToTicketsFile (ticket: Ticket) =
    let filePath = "tickets.json"

    let tickets =
        if File.Exists(filePath) then
            let json = File.ReadAllText(filePath)
            JsonSerializer.Deserialize<Ticket list>(json)
        else
            []
    let updatedTickets = ticket :: tickets

    let updatedJson =
        JsonSerializer.Serialize(updatedTickets, JsonSerializerOptions(WriteIndented = true))

    File.WriteAllText(filePath, updatedJson)

let readSpecificTicket (day: string) (party: string) (seat: int*int) =
    let filePath = "tickets.json"

    if File.Exists(filePath) then
        let json = File.ReadAllText(filePath)
        let tickets = JsonSerializer.Deserialize<Ticket list>(json)

        // Filter the tickets based on the parameters
        tickets
        |> List.tryFind (fun ticket ->
            ticket.Day.Equals(day, System.StringComparison.OrdinalIgnoreCase) && 
            ticket.Party.Equals(party, System.StringComparison.OrdinalIgnoreCase) 
            && ticket.Seat = seat)
    else
        None