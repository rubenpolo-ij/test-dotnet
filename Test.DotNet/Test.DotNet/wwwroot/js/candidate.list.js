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
                $(`#candidate${id}`).remove();
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
        $.get("/candidate/list", (data) => {
            let listContainer = $("#candidates tbody");
            data.forEach((candidate) => {
                const id = candidate.idCandidate;
                const tr = createRow(id, candidate);
                listContainer.append(tr);
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

    const createRow = (id, candidate) => {
        let tr = $(document.createElement('tr'));
        tr.attr("id", `candidate${id}`)
        $(document.createElement('td')).append(candidate.name).appendTo(tr);
        $(document.createElement('td')).append(candidate.surname).appendTo(tr);
        $(document.createElement('td')).append(candidate.email).appendTo(tr);
        let actions = $(document.createElement('td')).addClass("text-right");

        let editAction = $(document.createElement('a')).addClass("btn btn-outline-primary mr-2").attr("href", `/candidate/edit/${id}`);
        let iconPencil = $(document.createElement('i')).addClass("bi bi-pencil");
        iconPencil.appendTo(editAction);
        editAction.appendTo(actions);

        let deleteAction = $(document.createElement('a')).addClass("btn btn-outline-danger delete-button").data("id", id).attr("href", `/candidate/delete/${id}`);
        let iconDelete = $(document.createElement('i')).addClass("bi bi-person-x-fill");
        iconDelete.appendTo(deleteAction);
        deleteAction.appendTo(actions);

        actions.appendTo(tr);

        return tr;
    }

    cleanLocalStorage();
    getList();

});