module Data
open System.IO
open System.Text.Json
open System.Text.Json.Serialization

type CinemaData(day: string, parties: string, rows: int, cols: int) =
    let mutable seats = Array2D.create rows cols 0
    let day = day
    let parties = parties

    member this.Day
        with get() = day

    member this.Parties
        with get() = parties

    member this.Seats
        with get() = 
            // Convert Array2D to List of Lists for serialization
            [ for i in 0 .. rows - 1 -> [ for j in 0 .. cols - 1 -> seats.[i, j] ] ]

    member this.UpdateSeat(row: int, col: int) =
        if row >= 0 && row < rows && col >= 0 && col < cols then
            seats.[row, col] <- 1
        else
            failwith "Invalid row or column index"

let generateWeeklyBookings rows cols =
    let daysOfWeek = ["Saturday"; "Sunday"; "Monday"; "Tuesday"; "Wednesday"; "Thursday"; "Friday"]
    let partiesTimes = ["12 am"; "3 pm"; "6 pm"; "9 pm"]

    [ for day in daysOfWeek do
        for party in partiesTimes do
            yield CinemaData(day, party, rows, cols) ]

let saveDataToJson (cinemaData:CinemaData list) filename =
    let options = JsonSerializerOptions(WriteIndented = true)
    let json = JsonSerializer.Serialize(cinemaData, options)
    File.WriteAllText(filename, json)
    printfn "Data saved to %s" filename

