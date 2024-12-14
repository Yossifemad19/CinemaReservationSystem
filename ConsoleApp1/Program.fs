open System
open System.Windows.Forms
open System.Drawing
open System.IO

open Data
open Ticket

[<EntryPoint>]
let main argv=
    let cinemaDataPath="CinemaData.json"

    if not (File.Exists(cinemaDataPath)) then
        let data = generateWeeklyBookings 4 4
        saveDataToJson data cinemaDataPath


    let form=new Form(Text="Cinema Reservation System",Width=1000,Height=550)

    let panal = new Panel(Width=800,Height=550,AccessibleName="seatScrean",BackColor=Color.LightGray)

    

    let userNameInput=new TextBox(AccessibleName="UserNameInput",Width=100,
                                    Height=80,Location=new Point(850,50),Text="userName")

    let daysDropDown=new ComboBox(AccessibleName="daysDropDown",Width=100,
                                    Height=80,Location=new Point(850,250))

    daysDropDown.DataSource <- [|"Select Day";"Sunday";"Monday"|]
    daysDropDown.DropDownStyle <- ComboBoxStyle.DropDownList


    let partiesDropDown=new ComboBox(AccessibleName="partiesDropDown",Width=100,
                                    Height=80,Location=new Point(850,300))

    partiesDropDown.DataSource <- [|"Select Party";"9 AM";"1 PM"|]
    partiesDropDown.DropDownStyle <- ComboBoxStyle.DropDownList


    let Display_Seats(panal:Panel,n:int)=
            let mutable x=0
            let mutable y=0
            for i in 0..3 do
                for j in 0..3 do

                    let groupBox =new GroupBox(
                        Text = $"seat {i+1}.{j+1}",
                        Width=200,
                        Height=125,
                        Location=new Point(x,y)
                    )
                    let bookButton = new Button(
                        Text="Book",
                        Width=100,
                        Height=30,
                        Location=new Point(50,85),
                        BackColor=Color.Black,
                        ForeColor=Color.Wheat
                    )

                    bookButton.Click.Add(fun _ ->
                        let userName= userNameInput.Text
                        if userName="userName" || userName="" then
                            MessageBox.Show("Please provide a user name") |> ignore
                        else
                            let ticket = createTicket "sunday" "9 Am" (i,j) userName
                            appendToTicketsFile ticket

                    )


                    groupBox.Controls.Add(bookButton)
                    panal.Controls.Add(groupBox)
                    x <- x+200
                
                x <- 0
                y <- y+125


    let applyButton = new Button(AccessibleName="applyButton",Width=100,Height=30,
                                Location=new Point(850,400),Text="Apply",BackColor=Color.Gray)
    applyButton.Click.Add(fun _ ->
            //let day = daysDropDown.SelectedValue
            //MessageBox.Show($"day ->{day}")
            if File.Exists(cinemaDataPath) then
                MessageBox.Show("data file exist") 
                Display_Seats(panal,4)
    )


    //let Display_Seats(panal:Panel,n:int)=
    //        let mutable x=0
    //        let mutable y=0
    //        for i in 0..n do
    //            let groupBox =new GroupBox(
    //                Text = $"seat {i+1}",
    //                Width=200,
    //                Height=125,
    //                Location=new Point(x,y)
    //            )
    //            let bookButton = new Button(
    //                Text="Book",
    //                Width=100,
    //                Height=30,
    //                Location=new Point(50,85),
    //                BackColor=Color.Black,
    //                ForeColor=Color.Wheat
    //            )
    //            groupBox.Controls.Add(bookButton)
    //            panal.Controls.Add(groupBox)
    //            x <- x+200
    //            if x = 800 then
    //                x <- 0
    //                y <- y+125
                
    //Display_Seats(panal, 4)

    form.Controls.Add(panal)
    form.Controls.Add(userNameInput)

    form.Controls.Add(daysDropDown)
    form.Controls.Add(partiesDropDown)
    form.Controls.Add(applyButton)


    Application.Run(form)

    0