CreateStatusEffect("test")
        :Instantiate(
        function(effectBuilder, target, props)

            --local scoped variable here

            return effectBuilder
                    :OnReceiveDamage(
                    function(event)

                    end
            )       :OnDealDamage(
                    function(event)

                    end
            )       :OnTargetWithSpell(
                    function(event)

                    end
            )       :OnTargetedBySpell(
                    function(event)

                    end
            )       :OnCalculateStats(
                    function(stats)

                    end
            )       :OnNextTurn(
                    function()
                        return false;
                    end
            )       :OnTryCleanse(
                    function()
                        return false;
                    end
            )
        end
)
        :Build();