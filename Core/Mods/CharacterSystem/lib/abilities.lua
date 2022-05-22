CreateAbility("stats")
        :Instantiate(
        function(builder, config)
            return builder
                :Hidden(true)
                :OnCalculateStats(
                function(stats)
                    if(config.Data.health ~= nil) then
                        stats.Health = stats.Health + config.Data.health;
                    end
                    if(config.Data.mana ~= nil) then
                        stats.Mana = stats.Mana + config.Data.mana;
                    end
                    if(config.Data.armor ~= nil) then
                        stats.Armor = stats.Armor + config.Data.armor;
                    end
                    if(config.Data.charisma ~= nil) then
                        stats.Charisma = stats.Charisma + config.Data.charisma;
                    end
                    if(config.Data.wisdom ~= nil) then
                        stats.Wisdom = stats.Wisdom + config.Data.wisdom;
                    end
                    if(config.Data.intellect ~= nil) then
                        stats.Intellect = stats.Intellect + config.Data.intellect;
                    end
                    if(config.Data.dexterity ~= nil) then
                        stats.Dexterity = stats.Dexterity + config.Data.dexterity;
                    end
                    if(config.Data.constitution ~= nil) then
                        stats.Constitution = stats.Constitution + config.Data.constitution;
                    end
                    if(config.Data.strength ~= nil) then
                        stats.Strength = stats.Strength + config.Data.strength;
                    end
                end)
                :Build();

        end)
        :Build();