using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;

namespace Data.PlayerGameData
{
    public class PersistentPlayerGameData
    {
        private HumanFormSkins _selectedHumanFormSkin;
        private CarFormSkins _selectedCarFormSkin;
        private HelicopterFormSkins _selectedHelicopterFormSkin;
        private BoatFormSkins _selectedBoatFormSkin;

        private readonly List<HumanFormSkins> _openHumanFormSkins;
        private readonly List<CarFormSkins> _openCarFormSkins;
        private readonly List<HelicopterFormSkins> _openHelicopterFormSkins;
        private readonly List<BoatFormSkins> _openBoatFormSkins;

        private int _money;
        
        public DateTime? LastClaimTime { get; set; }

        public PersistentPlayerGameData()
        {
            _money = 0;
            _selectedHumanFormSkin = HumanFormSkins.White;
            _selectedCarFormSkin = CarFormSkins.SportWhite;
            _selectedHelicopterFormSkin = HelicopterFormSkins.Scout;
            _selectedBoatFormSkin = BoatFormSkins.Boat1;

            _openHumanFormSkins = new() {_selectedHumanFormSkin};
            _openCarFormSkins = new() {_selectedCarFormSkin};
            _openHelicopterFormSkins = new() {_selectedHelicopterFormSkin};
            _openBoatFormSkins = new() {_selectedBoatFormSkin};
        }

        [JsonConstructor]
        public PersistentPlayerGameData(int money, HumanFormSkins selectedHumanFormSkin, CarFormSkins selectedCarFormSkin,
            HelicopterFormSkins selectedHelicopterFormSkin, BoatFormSkins selectedBoatFormSkin,
            List<HumanFormSkins> openHumanFormSkins, List<CarFormSkins> openCarFormSkins,
            List<HelicopterFormSkins> openHelicopterFormSkins, List<BoatFormSkins> openBoatFormSkins)
        {
            Money = money;

            _selectedHumanFormSkin = selectedHumanFormSkin;
            _selectedCarFormSkin = selectedCarFormSkin;
            _selectedHelicopterFormSkin = selectedHelicopterFormSkin;
            _selectedBoatFormSkin = selectedBoatFormSkin;

            _openHumanFormSkins = new(openHumanFormSkins);
            _openCarFormSkins = new(openCarFormSkins);
            _openHelicopterFormSkins = new(openHelicopterFormSkins);
            _openBoatFormSkins = new(openBoatFormSkins);
        }

        public int Money
        {
            get => _money;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException(nameof(value));
                }

                _money = value;
            }
        }

        public HumanFormSkins SelectedHumanFormSkin
        {
            get => _selectedHumanFormSkin;
            set
            {
                if (_openHumanFormSkins.Contains(value) == false)
                {
                    throw new ArgumentException(nameof(value));
                }

                _selectedHumanFormSkin = value;
            }
        }
        
        public CarFormSkins SelectedCarFormSkin
        {
            get => _selectedCarFormSkin;
            set
            {
                if (_openCarFormSkins.Contains(value) == false)
                {
                    throw new ArgumentException(nameof(value));
                }

                _selectedCarFormSkin = value;
            }
        }
        
        public HelicopterFormSkins SelectedHelicopterFormSkin
        {
            get => _selectedHelicopterFormSkin;
            set
            {
                if (_openHelicopterFormSkins.Contains(value) == false)
                {
                    throw new ArgumentException(nameof(value));
                }

                _selectedHelicopterFormSkin = value;
            }
        }
        
        public BoatFormSkins SelectedBoatFormSkin
        {
            get => _selectedBoatFormSkin;
            set
            {
                if (_openBoatFormSkins.Contains(value) == false)
                {
                    throw new ArgumentException(nameof(value));
                }

                _selectedBoatFormSkin = value;
            }
        }

        public IEnumerable<HumanFormSkins> OpenHumanFormSkins => _openHumanFormSkins;
        public IEnumerable<CarFormSkins> OpenCarFormSkins => _openCarFormSkins;
        public IEnumerable<HelicopterFormSkins> OpenHelicopterFormSkins => _openHelicopterFormSkins;
        public IEnumerable<BoatFormSkins> OpenBoatFormSkins => _openBoatFormSkins;

        public void OpenHumanFormSkin(HumanFormSkins skin)
        {
            if (_openHumanFormSkins.Contains(skin))
            {
                throw new ArgumentException(nameof(skin));
            }
            
            _openHumanFormSkins.Add(skin);
        }
        
        public void OpenCarFormSkin(CarFormSkins skin)
        {
            if (_openCarFormSkins.Contains(skin))
            {
                throw new ArgumentException(nameof(skin));
            }
            
            _openCarFormSkins.Add(skin);
        }
        
        public void OpenHelicopterFormSkin(HelicopterFormSkins skin)
        {
            if (_openHelicopterFormSkins.Contains(skin))
            {
                throw new ArgumentException(nameof(skin));
            }
            
            _openHelicopterFormSkins.Add(skin);
        }
        
        public void OpenBoatFormSkin(BoatFormSkins skin)
        {
            if (_openBoatFormSkins.Contains(skin))
            {
                throw new ArgumentException(nameof(skin));
            }
            
            _openBoatFormSkins.Add(skin);
        }
    }
}