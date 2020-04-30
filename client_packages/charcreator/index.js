// shitcode will be better in the future
const NativeUI = require("nativeui");
const Data = require("charcreator/data");

const Menu = NativeUI.Menu;
const UIMenuItem = NativeUI.UIMenuItem;
const UIMenuListItem = NativeUI.UIMenuListItem;
const Point = NativeUI.Point;
const ItemsCollection = NativeUI.ItemsCollection;
const Color = NativeUI.Color;

const creatorCoords = {
    camera: new mp.Vector3(402.8664, -997.5515, -98.5),
    cameraLookAt: new mp.Vector3(402.8664, -996.4108, -98.5)
};

const localPlayer = mp.players.local;

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function colorForOverlayIdx(index) {
    let color;

    switch (index) {
        case 1:
            color = beardColorItem.Index;
        break;

        case 2:
            color = eyebrowColorItem.Index;
        break;

        case 5:
            color = blushColorItem.Index;
        break;

        case 8:
            color = lipstickColorItem.Index;
        break;

        case 10:
            color = chestHairColorItem.Index;
        break;

        default:
            color = 0;
    }

    return color;
}

function updateParents() {
    localPlayer.setHeadBlendData(
        // shape
        motherItem.Index,
        fatherItem.Index,
        0,

        // skin
        motherItem.Index,
        fatherItem.Index,
        0,

        // mixes
        similarityItem.Index * 0.01,
        skinSimilarityItem.Index * 0.01,
        0.0,

        false
    );
}

function updateFaceFeature(index) {
    localPlayer.setFaceFeature(index, parseFloat(featureItems[index].SelectedValue));
}

function updateAppearance(index) {
    let overlayID = (appearanceItems[index].Index == 0) ? 255 : appearanceItems[index].Index - 1;
    localPlayer.setHeadOverlay(index, overlayID, appearanceOpacityItems[index].Index * 0.01, colorForOverlayIdx(index), 0);
}

function updateHairAndColors() {
    localPlayer.setComponentVariation(2, Data.hairList[currentGender][hairItem.Index].ID, 0, 2);
    localPlayer.setHairColor(hairColorItem.Index, hairHighlightItem.Index);
    localPlayer.setEyeColor(eyeColorItem.Index);
    localPlayer.setHeadOverlayColor(1, 1, beardColorItem.Index, 0);
    localPlayer.setHeadOverlayColor(2, 1, eyebrowColorItem.Index, 0);
    localPlayer.setHeadOverlayColor(5, 2, blushColorItem.Index, 0);
    localPlayer.setHeadOverlayColor(8, 2, lipstickColorItem.Index, 0);
    localPlayer.setHeadOverlayColor(10, 1, chestHairColorItem.Index, 0);
}

function applyCreatorOutfit() {
    if (currentGender == 0) {
        localPlayer.setDefaultComponentVariation();
        localPlayer.setComponentVariation(3, 15, 0, 2);
        localPlayer.setComponentVariation(4, 21, 0, 2);
        localPlayer.setComponentVariation(6, 34, 0, 2);
        localPlayer.setComponentVariation(8, 15, 0, 2);
        localPlayer.setComponentVariation(11, 15, 0, 2);
    } else {
        localPlayer.setDefaultComponentVariation();
        localPlayer.setComponentVariation(3, 15, 0, 2);
        localPlayer.setComponentVariation(4, 10, 0, 2);
        localPlayer.setComponentVariation(6, 35, 0, 2);
        localPlayer.setComponentVariation(8, 15, 0, 2);
        localPlayer.setComponentVariation(11, 15, 0, 2);
    }
}

function fillHairMenu() {
    hairItem = new UIMenuListItem("Haare", "Die Haare deines Charakters.", new ItemsCollection(Data.hairList[currentGender].map(h => h.Name)));
    creatorHairMenu.AddItem(hairItem);

    hairColorItem = new UIMenuListItem("Haarfarbe", "Die Haarfarbe deines Charakters.", new ItemsCollection(hairColors));
    creatorHairMenu.AddItem(hairColorItem);

    hairHighlightItem = new UIMenuListItem("Haar-Highlight-Farbe", "Die Haarfarbe deines Charakters wird hervorgehoben.", new ItemsCollection(hairColors));
    creatorHairMenu.AddItem(hairHighlightItem);

    eyebrowColorItem = new UIMenuListItem("Augenbrauenfarbe", "Die Augenbrauenfarbe deines Charakters.", new ItemsCollection(hairColors));
    creatorHairMenu.AddItem(eyebrowColorItem);

    beardColorItem = new UIMenuListItem("Gesichtshaarfarbe", "Die Gesichtshaarfarbe deines Charakters.", new ItemsCollection(hairColors));
    creatorHairMenu.AddItem(beardColorItem);

    eyeColorItem = new UIMenuListItem("Augenfarbe", "Die Augenfarbe deines Charakters.", new ItemsCollection(Data.eyeColors));
    creatorHairMenu.AddItem(eyeColorItem);

    blushColorItem = new UIMenuListItem("Erröten", "Die Rouge-Farbe deines Charakters.", new ItemsCollection(blushColors));
    creatorHairMenu.AddItem(blushColorItem);

    lipstickColorItem = new UIMenuListItem("Lippenstiftfarbe", "Die Lippenstiftfarbe deines Charakters.", new ItemsCollection(lipstickColors));
    creatorHairMenu.AddItem(lipstickColorItem);

    chestHairColorItem = new UIMenuListItem("Brust Haarfarbe", "Die Brust Haarfarbe deines Charakters.", new ItemsCollection(hairColors));
    creatorHairMenu.AddItem(chestHairColorItem);

    creatorHairMenu.AddItem(new UIMenuItem("Randomize", "~r~Randomisiert Ihre Haare und Farben."));
    creatorHairMenu.AddItem(new UIMenuItem("Reset", "~r~Setzt Ihre Haare und Farben zurück."));
}

function resetParentsMenu(refresh = false) {
    fatherItem.Index = 0;
    motherItem.Index = 0;
    similarityItem.Index = 50;
    skinSimilarityItem.Index = 50;

    updateParents();
    if (refresh) creatorParentsMenu.RefreshIndex();
}

function resetFeaturesMenu(refresh = false) {
    for (let i = 0; i < Data.featureNames.length; i++) {
        featureItems[i].Index = 100;
        updateFaceFeature(i);
    }

    if (refresh) creatorFeaturesMenu.RefreshIndex();
}

function resetAppearanceMenu(refresh = false) {
    for (let i = 0; i < Data.appearanceNames.length; i++) {
        appearanceItems[i].Index = 0;
        appearanceOpacityItems[i].Index = 100;
        updateAppearance(i);
    }

    if (refresh) creatorAppearanceMenu.RefreshIndex();
}

function resetHairAndColorsMenu(refresh = false) {
    hairItem.Index = 0;
    hairColorItem.Index = 0;
    hairHighlightItem.Index = 0;
    eyebrowColorItem.Index = 0;
    beardColorItem.Index = 0;
    eyeColorItem.Index = 0;
    blushColorItem.Index = 0;
    lipstickColorItem.Index = 0;
    chestHairColorItem.Index = 0;
    updateHairAndColors();

    if (refresh) creatorHairMenu.RefreshIndex();
}

let currentGender = 0;
let creatorMenus = [];
let creatorCamera;

// color arrays
let hairColors = [];
for (let i = 0; i < Data.maxHairColor; i++) hairColors.push(i.toString());

let blushColors = [];
for (let i = 0; i < Data.maxBlushColor; i++) blushColors.push(i.toString());

let lipstickColors = [];
for (let i = 0; i < Data.maxLipstickColor; i++) lipstickColors.push(i.toString());

// CREATOR MAIN
let creatorMainMenu = new Menu("Creator", "", new Point(50, 50));
let genderItem = new UIMenuListItem("Geschlecht", "~r~Auchtung deine Anpassungen werden zurückgesetzt", new ItemsCollection(["Männlich", "Weiblich"]));
creatorMainMenu.AddItem(genderItem);
creatorMainMenu.AddItem(new UIMenuItem("Eltern", "Die Eltern deines Charakters."));
creatorMainMenu.AddItem(new UIMenuItem("Gesicht", "Die Gesichtszüge deines Charakters."));
creatorMainMenu.AddItem(new UIMenuItem("Aussehen", "Das Aussehen deines Charakters."));
creatorMainMenu.AddItem(new UIMenuItem("Haare", "Die Haare und Haarfarben deines Charakter."));

let angles = [];
for (let i = -180.0; i <= 180.0; i += 5.0) angles.push(i.toFixed(1));
let angleItem = new UIMenuListItem("Winkel", "", new ItemsCollection(angles));
creatorMainMenu.AddItem(angleItem);

let saveItem = new UIMenuItem("Speichern", "Save all changes.");
saveItem.BackColor = new Color(13, 71, 161);
saveItem.HighlightedBackColor = new Color(25, 118, 210);
creatorMainMenu.AddItem(saveItem);

creatorMainMenu.forceOpen = false;
creatorMainMenu.MenuClose.on(()  => {
    if (creatorMainMenu.forceOpen) {
        creatorMainMenu.Open();
    }
    
});


creatorMainMenu.ListChange.on((item, listIndex) => {
    if (item == genderItem) {
        currentGender = listIndex;
        let male = listIndex == 0;
        mp.events.callRemote("creator_GenderChange", male);
    } else if (item == angleItem) {
        localPlayer.setHeading(parseFloat(angleItem.SelectedValue));
        localPlayer.clearTasksImmediately();
    }
});

creatorMainMenu.ItemSelect.on((item, index) => {
    switch (index) {
        case 1:
            creatorMainMenu.Visible = false;
            creatorParentsMenu.Visible = true;
        break;

        case 2:
            creatorMainMenu.Visible = false;
            creatorFeaturesMenu.Visible = true;
        break;

        case 3:
            creatorMainMenu.Visible = false;
            creatorAppearanceMenu.Visible = true;
        break;

        case 4:
            creatorMainMenu.Visible = false;
            creatorHairMenu.Visible = true;
        break;

        case 6:
            let parentData = {
                ShapeFirst: fatherItem.Index,
                ShapeSecond: motherItem.Index,
                ShapeThird: 0,
                SkinFirst: fatherItem.Index,
                SkinSecond: motherItem.Index,
                SkinThird: 0,
                ShapeMix: similarityItem.Index * 0.01,
                SkinMix: skinSimilarityItem.Index * 0.01,
                ThirdMix: 0
            };

            let featureData = [];
            for (let i = 0; i < featureItems.length; i++) featureData.push(parseFloat(featureItems[i].SelectedValue));

            let headOverlays = {};
            for (let i = 0; i < appearanceItems.length; i++) headOverlays[i] = {Index: ((appearanceItems[i].Index == 0) ? 255 : appearanceItems[i].Index - 1), Opacity: appearanceOpacityItems[i].Index * 0.01, Color: 0, SecondaryColor: 0};

            let hairAndColors = [
                Data.hairList[currentGender][hairItem.Index].ID,
                hairColorItem.Index,
                hairHighlightItem.Index,
                eyebrowColorItem.Index,
                beardColorItem.Index,
                eyeColorItem.Index,
                blushColorItem.Index,
                lipstickColorItem.Index,
                chestHairColorItem.Index
            ];
			
			let hair = Data.hairList[currentGender][hairItem.Index].ID;

            let eyeColor = eyeColorItem.Index;
            let hairColor = hairColorItem.Index;
            let hightlightColor = hairHighlightItem.Index;

            let decorations = [];

            let male = currentGender == 0;

            mp.events.callRemote("login.character.create", hair,
                male, JSON.stringify(parentData), eyeColor, hairColor, hightlightColor,
                JSON.stringify(featureData), JSON.stringify(headOverlays), JSON.stringify(decorations)
            );
            break;
    }
});

creatorMainMenu.MenuClose.on(() => {
    mp.events.callRemote("creator_Leave");
});

creatorMainMenu.Visible = false;
creatorMenus.push(creatorMainMenu);
// CREATOR MAIN END

// CREATOR PARENTS
let similarities = [];
for (let i = 0; i <= 100; i++) similarities.push(i + "%");

let creatorParentsMenu = new Menu("Eltern", "", new Point(50, 50));
let fatherItem = new UIMenuListItem("Vater", "Der Vater deines Charakters.", new ItemsCollection(Data.fatherNames));
let motherItem = new UIMenuListItem("Mutter", "Die Mutter deines Charakters", new ItemsCollection(Data.motherNames));
let similarityItem = new UIMenuListItem("Ähnlichkeit", "Ähnlichkeit mit Eltern.\n(niedriger = weiblich, höher = männlich)", new ItemsCollection(similarities));
let skinSimilarityItem = new UIMenuListItem("Hautfarbe", "Hautfarbenähnlichkeit zu den Eltern.\n(niedriger = mutter's, höher = vater's)", new ItemsCollection(similarities));
creatorParentsMenu.AddItem(fatherItem);
creatorParentsMenu.AddItem(motherItem);
creatorParentsMenu.AddItem(similarityItem);
creatorParentsMenu.AddItem(skinSimilarityItem);
creatorParentsMenu.AddItem(new UIMenuItem("Randomize", "~r~Randomisiert deine Eltern."));
creatorParentsMenu.AddItem(new UIMenuItem("Reset", "~r~Setzt deine Eltern zurück."));

similarityItem.Index = 50;
skinSimilarityItem.Index = 50;

creatorParentsMenu.ItemSelect.on((item, index) => {
    switch (item.Text) {
        case "Randomize": //Verändern?
            fatherItem.Index = getRandomInt(0, Data.fathers.length - 1);
            motherItem.Index = getRandomInt(0, Data.mothers.length - 1);
            similarityItem.Index = getRandomInt(0, 100);
            skinSimilarityItem.Index = getRandomInt(0, 100);
            updateParents();
            break;

        case "Reset":
            resetParentsMenu();
            break;
    }
});

creatorParentsMenu.ListChange.on((item, listIndex) => {
    updateParents();
});

creatorParentsMenu.ParentMenu = creatorMainMenu;
creatorParentsMenu.Visible = false;
creatorMenus.push(creatorParentsMenu);
// CREATOR PARENTS END

// CREATOR FEATURES
let featureItems = [];
let features = [];
for (let i = -1.0; i <= 1.01; i += 0.01) features.push(i.toFixed(2));

let creatorFeaturesMenu = new Menu("Features", "", new Point(50, 50));

for (let i = 0; i < Data.featureNames.length; i++) {
    let tempFeatureItem = new UIMenuListItem(Data.featureNames[i], "", new ItemsCollection(features));
    tempFeatureItem.Index = 100;
    featureItems.push(tempFeatureItem);
    creatorFeaturesMenu.AddItem(tempFeatureItem);
}

creatorFeaturesMenu.AddItem(new UIMenuItem("Randomize", "~r~Randomisiert Ihre Funktionen."));
creatorFeaturesMenu.AddItem(new UIMenuItem("Reset", "~r~Setzt Ihre Funktionen zurück."));

creatorFeaturesMenu.ItemSelect.on((item, index) => {
    switch (item.Text) {
        case "Randomize":
            for (let i = 0; i < Data.featureNames.length; i++) {
                featureItems[i].Index = getRandomInt(0, 200);
                updateFaceFeature(i);
            }
        break;

        case "Reset":
            resetFeaturesMenu();
        break;
    }
});

creatorFeaturesMenu.ListChange.on((item, listIndex) => {
    updateFaceFeature(featureItems.indexOf(item));
});

creatorFeaturesMenu.ParentMenu = creatorMainMenu;
creatorFeaturesMenu.Visible = false;
creatorMenus.push(creatorFeaturesMenu);
// CREATOR FEATURES END

// CREATOR APPEARANCE
let appearanceItems = [];
let appearanceOpacityItems = [];
let opacities = [];
for (let i = 0; i <= 100; i++) opacities.push(i + "%");

let creatorAppearanceMenu = new Menu("Appearance", "", new Point(50, 50));

for (let i = 0; i < Data.appearanceNames.length; i++) {
    let items = [];
    for (let j = 0, max = mp.game.ped.getNumHeadOverlayValues(i); j <= max; j++) items.push((Data.appearanceItemNames[i][j] === undefined) ? j.toString() : Data.appearanceItemNames[i][j]);

    let tempAppearanceItem = new UIMenuListItem(Data.appearanceNames[i], "", new ItemsCollection(items));
    appearanceItems.push(tempAppearanceItem);
    creatorAppearanceMenu.AddItem(tempAppearanceItem);

    let tempAppearanceOpacityItem = new UIMenuListItem(Data.appearanceNames[i] + " Opacity", "", new ItemsCollection(opacities));
    tempAppearanceOpacityItem.Index = 100;
    appearanceOpacityItems.push(tempAppearanceOpacityItem);
    creatorAppearanceMenu.AddItem(tempAppearanceOpacityItem);
}

creatorAppearanceMenu.AddItem(new UIMenuItem("Randomize", "~r~Zufälliges Erscheinungsbild."));
creatorAppearanceMenu.AddItem(new UIMenuItem("Reset", "~r~Setzt dein Aussehen zurück."));

creatorAppearanceMenu.ItemSelect.on((item, index) => {
    switch (item.Text) {
        case "Randomize":
            for (let i = 0; i < Data.appearanceNames.length; i++) {
                appearanceItems[i].Index = getRandomInt(0, mp.game.ped.getNumHeadOverlayValues(i) - 1);
                appearanceOpacityItems[i].Index = getRandomInt(0, 100);
                updateAppearance(i);
            }
        break;

        case "Reset":
            resetAppearanceMenu();
        break;
    }
});

creatorAppearanceMenu.ListChange.on((item, listIndex) => {
    let idx = (creatorAppearanceMenu.CurrentSelection % 2 == 0) ? (creatorAppearanceMenu.CurrentSelection / 2) : Math.floor(creatorAppearanceMenu.CurrentSelection / 2);
    updateAppearance(idx);
});

creatorAppearanceMenu.ParentMenu = creatorMainMenu;
creatorAppearanceMenu.Visible = false;
creatorMenus.push(creatorAppearanceMenu);
// CREATOR APPEARANCE END

// CREATOR HAIR & COLORS
let hairItem;
let hairColorItem;
let hairHighlightItem;
let eyebrowColorItem;
let beardColorItem;
let eyeColorItem;
let blushColorItem;
let lipstickColorItem;
let chestHairColorItem;

creatorHairMenu = new Menu("Hair & Colors", "", new Point(50, 50));
fillHairMenu();



creatorHairMenu.ItemSelect.on((item, index) => {
    switch (item.Text) {
        case "Randomize":
            hairItem.Index = getRandomInt(0, Data.hairList[currentGender].length - 1);
            hairColorItem.Index = getRandomInt(0, Data.maxHairColor);
            hairHighlightItem.Index = getRandomInt(0, Data.maxHairColor);
            eyebrowColorItem.Index = getRandomInt(0, Data.maxHairColor);
            beardColorItem.Index = getRandomInt(0, Data.maxHairColor);
            eyeColorItem.Index = getRandomInt(0, Data.maxEyeColor);
            blushColorItem.Index = getRandomInt(0, Data.maxBlushColor);
            lipstickColorItem.Index = getRandomInt(0, Data.maxLipstickColor);
            chestHairColorItem.Index = getRandomInt(0, Data.maxHairColor);
            updateHairAndColors();
        break;

        case "Reset":
            resetHairAndColorsMenu();
        break;
    }
});

creatorHairMenu.ListChange.on((item, listIndex) => {
    if (item == hairItem) {
        let hairStyle = Data.hairList[currentGender][listIndex];
        localPlayer.setComponentVariation(2, hairStyle.ID, 0, 2);
    } else {
        switch (creatorHairMenu.CurrentSelection) {
            case 1: // hair color
                localPlayer.setHairColor(listIndex, hairHighlightItem.Index);
            break;

            case 2: // hair highlight color
                localPlayer.setHairColor(hairColorItem.Index, listIndex);
            break;

            case 3: // eyebrow color
                localPlayer.setHeadOverlayColor(2, 1, listIndex, 0);
            break;

            case 4: // facial hair color
                localPlayer.setHeadOverlayColor(1, 1, listIndex, 0);
            break;

            case 5: // eye color
                localPlayer.setEyeColor(listIndex);
            break;

            case 6: // blush color
                localPlayer.setHeadOverlayColor(5, 2, listIndex, 0);
            break;

            case 7: // lipstick color
                localPlayer.setHeadOverlayColor(8, 2, listIndex, 0);
            break;

            case 8: // chest hair color
                localPlayer.setHeadOverlayColor(10, 1, listIndex, 0);
            break;
        }
    }
});

creatorHairMenu.ParentMenu = creatorMainMenu;
creatorHairMenu.Visible = false;
creatorMenus.push(creatorHairMenu);
// CREATOR HAIR & COLORS END

// EVENTS

mp.events.add("creator_GenderChanged", () => {
    localPlayer.clearTasksImmediately();
    applyCreatorOutfit();
    angleItem.Index = 0;
    resetParentsMenu(true);
    resetFeaturesMenu(true);
    resetAppearanceMenu(true);

    creatorHairMenu.Clear();
    fillHairMenu();
    creatorHairMenu.RefreshIndex();
});

mp.events.add("toggleCreator", (active) => {
    if (active) {
        creatorMainMenu.forceOpen = true;

        if (creatorCamera === undefined) {
            creatorCamera = mp.cameras.new("creatorCamera", creatorCoords.camera, new mp.Vector3(0, 0, 0), 45);
            creatorCamera.pointAtCoord(creatorCoords.cameraLookAt);
            creatorCamera.setActive(true);
        }
        localPlayer.setHeading(-185.0);
        creatorMainMenu.Visible = true;
        mp.gui.chat.show(false);
        mp.game.ui.displayRadar(false);
        mp.game.ui.displayHud(false);
        localPlayer.clearTasksImmediately();
        localPlayer.freezePosition(true);

        mp.game.cam.renderScriptCams(true, false, 0, true, false);
    } else {
        creatorMainMenu.forceOpen = false;
        for (let i = 0; i < creatorMenus.length; i++) creatorMenus[i].Visible = false;
        mp.gui.chat.show(true);
        mp.game.ui.displayRadar(true);
        mp.game.ui.displayHud(true);
        localPlayer.freezePosition(false);
        localPlayer.setDefaultComponentVariation();
        localPlayer.setComponentVariation(2, Data.hairList[currentGender][hairItem.Index].ID, 0, 2);

        mp.game.cam.renderScriptCams(false, false, 0, true, false);
    }
});