export class Sort {
    private sortOder: number = 1;

    constructor(){}

    public startSort(prop: string, sortType: string) {
        this.sortOder = 1;
        if (sortType === 'desc') {
            this.sortOder = -1;
        }
        return (a: any, b: any) => (a[prop] - b[prop]) * this.sortOder;
    }
}
