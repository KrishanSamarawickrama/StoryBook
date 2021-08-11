export function ParseWebApiErrors(response: any): string[] {
    const results: string[] = [];

    if (response.error) {
        if (typeof response.error === 'string') {
            results.push(response.error);
        } else if (Array.isArray(response.error)) {
            response.error.array.forEach((value: any) => {
                results.push(value);
            });
        }
    }

    return results;
}