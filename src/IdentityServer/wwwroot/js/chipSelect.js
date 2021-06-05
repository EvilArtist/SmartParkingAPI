window.onload = function () {
    var chipsCollections = document.querySelectorAll(".chips");
    var chipSelection = [...chipsCollections].map(x => new ChipSelection(x));
}


class ChipSelection {
    constructor(chipsCollection) {
        this.chipsCollection = chipsCollection;
        const parent = chipsCollection.parentNode;
        this.select = parent.querySelector("select");
        const options = this.select.querySelectorAll("option");
        for (var option of options) {
            let value = option.value;
            let selected = option.selected;
            let chip = this.chipsCollection.querySelector(`.chip[value=${value}`);
            if (selected) {
                chip.classList.add("selected");
            }
        }

        const chips = this.chipsCollection.querySelectorAll(".chip");
        for (var chip of chips) {
            chip.addEventListener("click", e => {
                let value = e.target.getAttribute("value");
                let option = this.select.querySelector(`option[value=${value}`);
                let selected = option.selected;
                if (selected) {
                    option.removeAttribute("selected");
                    e.target.classList.remove("selected");
                } else {
                    option.setAttribute("selected", true);
                    e.target.classList.add("selected");
                }
            })
        }
           
    }
}
