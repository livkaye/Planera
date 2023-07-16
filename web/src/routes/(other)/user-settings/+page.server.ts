import type {RequestEvent, ServerLoadEvent} from "@sveltejs/kit";
import {getUserClient} from "$lib/clients";
import {error, fail} from "@sveltejs/kit";
import {toProblemDetails} from "$lib/problemDetails";
import type {EditUserModel, ChangePasswordModel, AccountDto, SwaggerException} from "../../../gen/planeraClient";

export async function load({ cookies }: ServerLoadEvent) {
    let response: AccountDto;
    try {
        response = await getUserClient(cookies).getAccount();
    } catch (ex) {
        const problem = toProblemDetails(ex as SwaggerException);
        throw error(problem.status ?? 400, problem.summary);
    }

    return {
        account: structuredClone(response),
        error: false,
    };
}

export const actions = {
    update: async ({ request, cookies, params }: RequestEvent) => {
        const formData = await request.formData();
        try {
            await getUserClient(cookies).edit(
                {
                    username: formData.get("username"),
                    email: formData.get("email"),
                    avatar: formData.get("avatar"),
                } as EditUserModel,
            );
        } catch (ex) {
            const problem = toProblemDetails(ex as SwaggerException);

            return fail(400, {
                update: {
                    errors: problem?.errors,
                },
            });
        }
    },
    changePassword: async ({ request, cookies, params }: RequestEvent) => {
        const formData = await request.formData();
        try {
            await getUserClient(cookies).changePassword(
                {
                    currentPassword: formData.get("currentPassword"),
                    newPassword: formData.get("newPassword"),
                    confirmedPassword: formData.get("confirmedPassword"),
                } as ChangePasswordModel,
            );
        } catch (ex) {
            const problem = toProblemDetails(ex as SwaggerException);

            return fail(400, {
                changePassword: {
                    errors: problem?.errors,
                },
            });
        }
    },
};