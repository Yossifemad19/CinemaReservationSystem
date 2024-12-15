module Data
open System.IO
open System.Text.Json
open System.Text.Json.Serialization

type CinemaData(day: string, party: string, rows: int, cols: int) =
    let mutable seats = Array2D.create rows cols 0
    let day = day
    let party = party

    member this.Day
        with get() = day

    member this.Party
        with get() = party

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


// Function to retrieve seats from JSON file
let readCinemaData (day: string) (party: string) (filename: string) : Result<CinemaData, string> =
    try
        // Read and parse the JSON file
        let json = File.ReadAllText(filename)
        let records = JsonSerializer.Deserialize<CinemaData list>(json)
        
        // Find the matching record
        match records |> List.tryFind (fun record -> 
            record.Day.Equals(day, System.StringComparison.OrdinalIgnoreCase) && 
            record.Party.Equals(party, System.StringComparison.OrdinalIgnoreCase)) with
        | Some record -> Ok record
        | None -> Error (sprintf "No record found for day: %s, party: %s" day party)
    with
    | :? IOException as e -> 
        Error (sprintf "File Error: %s" e.Message)
    | :? JsonException as e ->
        Error (sprintf "JSON Parsing Error: %s" e.Message)
    | ex ->
        Error (sprintf "Unexpected Error: %s" ex.Message)

        

        // Function to update seats and overwrite JSON file




let updateSeats (day: string) (party: string) (filename: string) (updatedRecord: CinemaData) =
    try
        // Read and parse the JSON file
        let json = File.ReadAllText(filename)
        let records = JsonSerializer.Deserialize<CinemaData list>(json)

        if List.isEmpty records then
            printfn "No records found in the file."
        else
            // Update the matching record
            let updatedRecords =
                records 
                |> List.map (fun record ->
                    if record.Day.Equals(day, System.StringComparison.OrdinalIgnoreCase) &&
                       record.Party.Equals(party, System.StringComparison.OrdinalIgnoreCase) then
                        updatedRecord // Replace with the updated record
                    else
                        record // Keep other records unchanged
                )

            // Write the updated records back to the file
            let updatedJson = JsonSerializer.Serialize(updatedRecords, JsonSerializerOptions(WriteIndented = true))
            File.WriteAllText(filename, updatedJson)
            printfn "Seats updated successfully."

    with
    | :? IOException as e ->
        printfn "File Error: %s" e.Message
    | :? JsonException as e ->
        printfn "JSON Parsing Error: %s" e.Message
    | ex ->
        printfn "Unexpected Error: %s" ex.Message




