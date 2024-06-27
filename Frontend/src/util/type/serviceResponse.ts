type OkServiceResponse<T> = {
    ok: true;
    data: T|null;
    errors: [];
}

type NotOkServiceResponse = {
    ok: false;
    data: null;
    errors: string[];
}

export type ServiceResponse<T> = OkServiceResponse<T> | NotOkServiceResponse;