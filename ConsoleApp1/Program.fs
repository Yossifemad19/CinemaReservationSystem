open System
open System.Windows.Forms
open System.Drawing


[<EntryPoint>]
let main argv=
    
    let Display_Seats(panal:Panel,n:int)=
            let mutable x=0
            let mutable y=0
            for i in 0..n do
                let groupBox =new GroupBox(
                    Text = $"seat {i+1}",
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
                groupBox.Controls.Add(bookButton)
                panal.Controls.Add(groupBox)
                x <- x+200
                if x = 800 then
                    x <- 0
                    y <- y+125

    let form=new Form(Text="Cinema Reservation System",Width=1000,Height=550)

    let panal = new Panel(Width=800,Height=550,AccessibleName="seatScrean",BackColor=Color.LightGray)

    //let daysDropDown= new ToolStripDropDownMenu(AccessibleName="daysDropDown",Width=100,Height=80,Text="day")
    ////daysDropDown.Text <- ""
    //daysDropDown.Location <- new Point(850,50) 

    let daysDropDown=new ComboBox(AccessibleName="daysDropDown",Width=100,
                                    Height=80,Location=new Point(850,50))

    daysDropDown.DataSource <- [|"Select Day";"Sunday";"Monday"|]
    daysDropDown.DropDownStyle <- ComboBoxStyle.DropDownList


    let partiesDropDown=new ComboBox(AccessibleName="partiesDropDown",Width=100,
                                    Height=80,Location=new Point(850,200))

    partiesDropDown.DataSource <- [|"Select Party";"9 AM";"1 PM"|]
    partiesDropDown.DropDownStyle <- ComboBoxStyle.DropDownList


    let applyButton = new Button(AccessibleName="applyButton",Width=100,Height=30,
                                Location=new Point(850,300),Text="Apply",BackColor=Color.Gray)
    applyButton.Click.Add(fun _ ->
            let day = daysDropDown.SelectedValue
            MessageBox.Show($"day ->{day}")
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
    form.Controls.Add(daysDropDown)
    form.Controls.Add(partiesDropDown)
    form.Controls.Add(applyButton)


    Application.Run(form)

    0