$(function () {

    $(document).on('click', '.delete-button', (e) => {
        e.stopPropagation();
        e.preventDefault();
        const $button = $(e.target).is("a") ? $(e.target) : $(e.target).closest('a');
        const $modal = $("#confirmDeletionDialog");

        const action = $button.attr("href");
        const id = $button.data("id");
        const value = JSON.stringify({ id, action });
        localStorage.setItem("deletionAction", value);
        $modal.modal('show');
    });

    $('#confirmDelete').click((e) => {
        e.stopPropagation();
        const $button = $(e.target).is("button") ? $(e.target) : $(e.target).closest('button');
        const $modal = $("#confirmDeletionDialog");

        disableButton($button);
        const url = get("action");
        const id = get("id");
        $.post(url)
            .then(function (response) {
                $(`#experience${id}`).remove();
                cleanLocalStorage();
                enableButton($button);
                $modal.modal('hide');
            })
            .catch((error) => {
                cleanLocalStorage();
                enableButton($button);
                $modal.modal('hide');
                bs4Toast.error('An error occurred', error.responseText);
            });

    });

    const getList = () => {
        const idCandidate = $('#inputCandidateId').val();
        const url = `${idCandidate}/Experience/List/`;
        $.get(url, (data) => {
            let listContainer = $("#listContainer");
            data.forEach((experience) => {
                const id = experience.idCandidateExperience;
                const card = createExperienceCard(idCandidate, id, experience);
                listContainer.append(card);
            });

        })
            .catch((error) => {
                bs4Toast.error('An error occurred', error.responseText);
            });
    }

    const get = (property) => {
        let action = JSON.parse(localStorage.getItem("deletionAction"));

        return action[property];
    }

    const cleanLocalStorage = () => {
        localStorage.removeItem("deletionAction");
    }

    const disableButton = (button) => {
        button.addClass("disabled");
        button.children(".spinner-border").removeClass("d-none");
    }

    const enableButton = (button) => {
        button.children(".spinner-border").addClass("d-none");
        button.removeClass("disabled");
    }

    const createExperienceCard = (idCandidate, id, experience) => {
        let card = $(document.createElement('div')).addClass("card mb-2 card-experience").attr("id", `experience${id}`);
        let cardBody = $(document.createElement('div')).addClass("card-body");
        let cardTitle = $(document.createElement('h6')).addClass("card-title");
        let cardText = $(document.createElement('p')).addClass("card-text mt-4");
        let actions = $(document.createElement('div')).addClass("text-right ml-2 mr-2 mb-2");

        $(document.createElement('span')).addClass("float-left").append(`${experience.company} - ${experience.job}`).appendTo(cardTitle);
        $(document.createElement('span')).addClass("float-right").append(`${experience.beginDate} - ${experience.endDate}`).appendTo(cardTitle);
        cardTitle.appendTo(cardBody);

        $(document.createElement('small')).append(`$${experience.salary}`).appendTo(cardText);
        $(document.createElement('br')).appendTo(cardText);
        cardText.append(`${experience.description}`);
        cardText.appendTo(cardBody);
        cardBody.appendTo(card);

        let editAction = $(document.createElement('a')).addClass("btn btn-outline-primary experience-action mr-2").attr("href", `/Candidate/${idCandidate}/Experience/Edit/${id}`);
        let iconPencil = $(document.createElement('i')).addClass("bi bi-pencil");
        iconPencil.appendTo(editAction);
        editAction.appendTo(actions);

        let deleteAction = $(document.createElement('a')).addClass("btn btn-outline-danger delete-button experience-action").data("id", id).attr("href", `/Candidate/${idCandidate}/Experience/Delete/${id}`);
        let iconDelete = $(document.createElement('i')).addClass("bi bi-trash3");
        iconDelete.appendTo(deleteAction);
        deleteAction.appendTo(actions);

        actions.appendTo(card);

        return card;
    }

    cleanLocalStorage();
    getList();

});