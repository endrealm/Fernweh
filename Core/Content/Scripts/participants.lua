CreateParticipant("test")
        :Instantiate(function(builder)
    
            return builder
                    :Health(20)
                    :Mana(10)
                    :Build();
        end)
        :Build();